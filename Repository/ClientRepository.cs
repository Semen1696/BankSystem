using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Логика для работы с клиентом
    /// </summary>
    public class ClientRepository
    {
        public delegate void ClientHandler(string message);
        public event ClientHandler Notify;
        private SqlDataAdapter adapter;

        public ClientRepository()
        {
            PrepareClient();
        }
        /// <summary>
        /// Добавить нового клиента
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            
            DataRow r = BankContext.ClientTable.NewRow();

            r["Name"] = client.Name;
            r["Surname"] = client.Surname;
            r["Phone"] = client.Phone;
            r["Age"] = client.Age;
            r["ClientTypeId"] = client.ClientTypeId;

            BankContext.ClientTable.Rows.Add(r);
            adapter.Update(BankContext.ClientTable);

            Notify?.Invoke($"Добавлен пользователь {client.Name} {client.Surname}");
        }
        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(int id)
        {
            var accounts = BankContext.AccountTable.Select($"ClientId = {id}");
            var acc = new Account();
            foreach (var account in accounts)
            {
                acc.RemoveAccount(Convert.ToInt32(account["Id"]));
            }

            var rows = BankContext.ClientTable.Select($"Id = {id}");
            if(rows.Length > 0)
            {
                Notify?.Invoke($"Пользователь {rows[0]["Name"]} {rows[0]["Surname"]} удален");
                rows[0].Delete();
                adapter.Update(BankContext.ClientTable);
                
            }

           
        }
        /// <summary>
        /// Получить клиента по уникальному номеру
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetClientById(int id)
        {
            var rows = BankContext.ClientTable.Select($"Id = {id}");
            if (rows.Length > 0)
            {
                var selected = rows[0];
                var client = new Client
                {
                    Id = Convert.ToInt32(selected["Id"]),
                    Name = selected["Name"].ToString(),
                    Surname = selected["Surname"].ToString(),
                    Phone = selected["Phone"].ToString(),
                    Age = Convert.ToInt32(selected["Age"]),
                    ClientTypeId = Convert.ToInt32(selected["ClientTypeId"])
                };
                return client;
            }
            return null;
           
        }

        /// <summary>
        /// сохранить изменения
        /// </summary>
        public void SaveChanges()
        {
            adapter.Update(BankContext.ClientTable);
            Notify?.Invoke("Данные пользователя изменены");
        }

        private void PrepareClient()
        {
            adapter = new SqlDataAdapter
            {
                InsertCommand = new SqlCommand(SqlQueries.InsertClient, BankContext.con),
                UpdateCommand = new SqlCommand(SqlQueries.UpdateClient, BankContext.con),
                DeleteCommand = new SqlCommand(SqlQueries.DeleteClient, BankContext.con)
            };

            adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            adapter.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 20, "Name");
            adapter.InsertCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 20, "Surname");
            adapter.InsertCommand.Parameters.Add("@Phone", SqlDbType.NVarChar, 10, "Phone");
            adapter.InsertCommand.Parameters.Add("@Age", SqlDbType.Int, 4, "Age");
            adapter.InsertCommand.Parameters.Add("@ClientTypeId", SqlDbType.Int, 4, "ClientTypeId");

            adapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 20, "Name");
            adapter.UpdateCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 20, "Surname");
            adapter.UpdateCommand.Parameters.Add("@Phone", SqlDbType.NVarChar, 10, "Phone");
            adapter.UpdateCommand.Parameters.Add("@Age", SqlDbType.Int, 4, "Age");
            adapter.UpdateCommand.Parameters.Add("@ClientTypeId", SqlDbType.Int, 4, "ClientTypeId");

            adapter.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");
        }
        
    }
}
