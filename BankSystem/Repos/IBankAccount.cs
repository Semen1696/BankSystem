using System.Collections.ObjectModel;

namespace BankSystem.Models
{
    interface IBankAccount<out T>
    {
        BankAccount CreateAccount(Client client);
        void RemoveAccount(BankAccount bankAccount);
        void MakeDeposit(BankAccount bankAccount, decimal sum);
        void MakeTransfer(string accountNumberToTransfer, string accountNumberFromTransfer, decimal sum);
        ObservableCollection<BankAccount> GetAccountsByClientId(int id);
        BankAccount GetAccountByNumber(string number);
        void SaveAccount();

    }
}
