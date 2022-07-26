using BankSystem.Models;
using System;

namespace BankSystem.Repos
{
    /// <summary>
    /// Работа с депозитным счетом
    /// </summary>
    public class DepositAccount : Account
    {
        private readonly Repository<BankAccount> repository;
        private readonly string accountPath = "Accounts.json";
        public DepositAccount()
        {
            repository = new Repository<BankAccount>(accountPath);
        }
        public override BankAccount CreateAccount(Client client)
        {
            
            BankAccount bankAccount = new BankAccount
            {
                Number = RandomString(10),
                ClientId = client.Id,
                Amount = decimal.Zero,
                Createdate = DateTime.Now,
                AccountType = AccountType.Deposit
            };
            bankAccount.Id = repository.LastId(bankAccount) + 1;
            repository.AddItem(bankAccount);
            
            return bankAccount;
        }
    }
}
