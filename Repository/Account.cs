using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Взаимодействие со счетом клиента
    /// </summary>
    public class Account : IBankAccount
    {

        public delegate void AccountHandler(string message);
        public event AccountHandler NotifyEvent;
        public event AccountHandler Notify
        {
            add
            {
                NotifyEvent += value;
            }
            remove
            {
                NotifyEvent += value;
            }
        }
        protected SqlDataAdapter adapter;

        public Account()
        {
            PrepareAccount();
        }
        /// <summary>
        /// Создание счета
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public virtual void CreateAccount(Client client)
        {

            DataRow r = BankContext.AccountTable.NewRow();
            r["Number"] = RandomString(10);
            r["Amount"] = decimal.Zero;
            r["CreateDate"] = DateTime.Now;
            r["AccountType"] = (int)AccountType.Sample;
            r["ClientId"] = client.Id;

            BankContext.AccountTable.Rows.Add(r);
            SaveAccount();

            NotifyEvent?.Invoke($"Открыт счет #{r["Number"]} на имя {client.Name}");

        }

        /// <summary>
        /// Удаление счета
        /// </summary>
        /// <param name="bankAccount"></param>
        public virtual void RemoveAccount(int id)
        {
            var rows = BankContext.AccountTable.Select($"Id = {id}");
            if (rows.Length > 0)
            {
                NotifyEvent?.Invoke($"Закрыт счет #{rows[0]["Number"]}");
                rows[0].Delete();
                SaveAccount();
                
            }

        }
        /// <summary>
        /// Пополнеие счета
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <param name="sum"></param>
        public void MakeDeposit(int accountId, decimal sum)
        {
            var rows = BankContext.AccountTable.Select($"Id = {accountId}");
            if (rows.Length > 0)
            {
                var oldAmount = Convert.ToDecimal(rows[0]["Amount"]);
                var newAmount = oldAmount + sum;
                rows[0]["Amount"] = newAmount;
                SaveAccount();

                NotifyEvent?.Invoke($"Счет {rows[0]["Number"]} пополнен на {sum}");
            }

        }
        /// <summary>
        /// Перевод между счетами
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="sum"></param>
        public void MakeTransfer(string accountNumberToTransfer, string accountNumberFromTransfer, decimal sum)
        {
            var rows = BankContext.AccountTable.Select($"Number = '{accountNumberFromTransfer}'");
            if (rows.Length > 0)
            {
                var oldAmount = Convert.ToDecimal(rows[0]["Amount"]);
                var newAmount = oldAmount - sum;
                rows[0]["Amount"] = newAmount;
            }

            rows = BankContext.AccountTable.Select($"Number = '{accountNumberToTransfer}'");
            if (rows.Length > 0)
            {
                var oldAmount = Convert.ToDecimal(rows[0]["Amount"]);
                var newAmount = oldAmount + sum;
                rows[0]["Amount"] = newAmount;
            }

            SaveAccount();
            NotifyEvent?.Invoke($"Перевод {sum} со счета {accountNumberFromTransfer} на счет {accountNumberToTransfer}");
        }
        /// <summary>
        /// Сохранение изменений в файл 
        /// </summary>
        public void SaveAccount()
        {
            adapter.Update(BankContext.AccountTable);
        }

        /// <summary>
        /// Получение счета клиента по его уникальному номеру
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void FillAccountTableByClientId(int id)
        {
            BankContext.AccountTable = new DataTable();

            string sql = $@"SELECT * FROM Accounts WHERE ClientId={id}";
            adapter.SelectCommand = new SqlCommand(sql, BankContext.con);
            adapter.Fill(BankContext.AccountTable);
        }
        public DataRow GetAccountByNumber(string number)
        {
            DataTable dt = new DataTable();
            string sql = $@"SELECT * FROM Accounts WHERE Number='{number}'";
            adapter.SelectCommand = new SqlCommand(sql, BankContext.con);
            adapter.Fill(dt);
            
            
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// Генерация номера счета
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }

        private void PrepareAccount()
        {
            adapter = new SqlDataAdapter
            {
                InsertCommand = new SqlCommand(SqlQueries.InsertAccont, BankContext.con),
                UpdateCommand = new SqlCommand(SqlQueries.UpdateAccount, BankContext.con),
                DeleteCommand = new SqlCommand(SqlQueries.DeleteAccount, BankContext.con)
            };

            adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            adapter.InsertCommand.Parameters.Add("@Number", SqlDbType.NVarChar, 10, "Number");
            adapter.InsertCommand.Parameters.Add("@Amount", SqlDbType.Decimal, 20, "Amount");
            adapter.InsertCommand.Parameters.Add("@CreateDate", SqlDbType.DateTime, 10, "CreateDate");
            adapter.InsertCommand.Parameters.Add("@AccountType", SqlDbType.Int, 4, "AccountType");
            adapter.InsertCommand.Parameters.Add("@ClientId", SqlDbType.Int, 4, "ClientId");

            adapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").SourceVersion = DataRowVersion.Original;
            adapter.UpdateCommand.Parameters.Add("@Number", SqlDbType.NVarChar, 10, "Number");
            adapter.UpdateCommand.Parameters.Add("@Amount", SqlDbType.Decimal, 20, "Amount");
            adapter.UpdateCommand.Parameters.Add("@CreateDate", SqlDbType.DateTime, 10, "CreateDate");
            adapter.UpdateCommand.Parameters.Add("@AccountType", SqlDbType.Int, 4, "AccountType");
            adapter.UpdateCommand.Parameters.Add("@ClientId", SqlDbType.Int, 4, "ClientId");

            adapter.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");
        }
    }
}
