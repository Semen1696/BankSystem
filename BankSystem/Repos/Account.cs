using BankSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace BankSystem.Repos
{
    /// <summary>
    /// Взаимодействие со счетом клиента
    /// </summary>
    public class Account
    {
        private readonly Repository<BankAccount> repository;
        private readonly string accountPath = "Accounts.json";

        public delegate void AccountHandler(string message);
        public event AccountHandler Notify;

        public Account()
        {
            repository = new Repository<BankAccount>(accountPath);
        }
        /// <summary>
        /// Создание счета
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public virtual BankAccount CreateAccount(Client client)
        {
   
            BankAccount bankAccount = new BankAccount
            {
                Number = RandomString(10),
                ClientId = client.Id,
                Amount = decimal.Zero,
                Createdate = DateTime.Now,
                AccountType = AccountType.Sample
            };
            bankAccount.Id = repository.LastId(bankAccount) + 1;
            repository.AddItem(bankAccount);
            Notify?.Invoke($"Открыт счет #{bankAccount.Number} на имя {client.Name}");
            return bankAccount;
            
        }

        /// <summary>
        /// Удаление счета
        /// </summary>
        /// <param name="bankAccount"></param>
        public virtual void RemoveAccount(BankAccount bankAccount)
        {
            repository.RemoveItem(bankAccount);
            Notify?.Invoke($"Закрыт счет #{bankAccount.Number}");
        }
        /// <summary>
        /// Пополнеие счета
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <param name="sum"></param>
        public void MakeDeposit(BankAccount bankAccount, decimal sum)
        {
            bankAccount.Amount += sum;
            Save();
            Notify?.Invoke($"Счет {bankAccount.Number} пополнен на {sum}");
        }
        /// <summary>
        /// Перевод между счетами
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="sum"></param>
        public void MakeTransfer(string accountNumberToTransfer, string accountNumberFromTransfer, decimal sum)
        {
            GetAccountByNumber(accountNumberFromTransfer).Amount -= sum;
            GetAccountByNumber(accountNumberToTransfer).Amount += sum;           
            Save();
            Notify?.Invoke($"Перевод {sum} со счета {accountNumberFromTransfer} на счет {accountNumberToTransfer}");
        }
        /// <summary>
        /// Сохранение изменений в файл 
        /// </summary>
        public void Save()
        {
            repository.SaveItem();
        }

        /// <summary>
        /// Получение счета клиента по его уникальному номеру
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ObservableCollection<BankAccount> GetAccountsByClientId(int id)
        {
            ObservableCollection<BankAccount> accounts = repository.GetItems();
            ObservableCollection<BankAccount> selectedAccounts = new ObservableCollection<BankAccount>();
            foreach (var item in accounts)
            {
                if(item.ClientId == id)
                {
                    selectedAccounts.Add(item);
                }
            }
            return selectedAccounts;
        }

        /// <summary>
        /// Получение счета по номеру счета
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public BankAccount GetAccountByNumber(string number)
        {
            ObservableCollection<BankAccount> accounts = repository.GetItems();
            var account = accounts.FirstOrDefault(a => a.Number == number);
            return account;
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
    }
}
