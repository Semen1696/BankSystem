using Models;
using System.Data;
using static Logic.Account;

namespace Logic
{
    public interface IBankAccount
    {
        void CreateAccount(Client client);
        void RemoveAccount(int id);
        void MakeDeposit(int accountId, decimal sum);
        void MakeTransfer(string accountNumberToTransfer, string accountNumberFromTransfer, decimal sum);
        void FillAccountTableByClientId(int id);
        void SaveAccount();
        DataRow GetAccountByNumber(string number);
        event AccountHandler Notify;
    }
}
