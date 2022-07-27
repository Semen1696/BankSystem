using System.Collections.ObjectModel;
using Common;
using Models;

namespace Logic
{
    public class BankAccountProcess<T> : IBankAccount<T> where T : Account, new()
    {
        private readonly T _bankAccount;
        public BankAccountProcess()
        {
            _bankAccount = new T();
            _bankAccount.Notify += BankContext.Log;
        }

        public BankAccount CreateAccount(Client client)
        {    
            return _bankAccount.CreateAccount(client);

        }
        public void RemoveAccount(BankAccount bankAccount)
        {
            _bankAccount.RemoveAccount(bankAccount);
        }
        public void MakeDeposit(BankAccount bankAccount, decimal sum)
        {
            _bankAccount.MakeDeposit(bankAccount, sum);
        }
        public void MakeTransfer(string accountNumberToTransfer, string accountNumberFromTransfer, decimal sum)
        {
            _bankAccount.MakeTransfer(accountNumberToTransfer, accountNumberFromTransfer, sum);
        }
        public ObservableCollection<BankAccount> GetAccountsByClientId(int id)
        {
            return _bankAccount.GetAccountsByClientId(id);
        }
        public BankAccount GetAccountByNumber(string number)
        {
            return _bankAccount.GetAccountByNumber(number);
        }
        public void SaveAccount()
        {
            _bankAccount.Save();
        }

    }
}
