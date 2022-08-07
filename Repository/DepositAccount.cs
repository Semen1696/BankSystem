using Common;
using Models;
using System;
using System.Data;

namespace Logic
{
    /// <summary>
    /// Работа с депозитным счетом
    /// </summary>
    public class DepositAccount : Account
    {
        public DepositAccount()
        {
            
        }
        public override void CreateAccount(Client client)
        {
            DataRow r = BankContext.AccountTable.NewRow();
            r["Number"] = RandomString(10);
            r["Amount"] = decimal.Zero;
            r["CreateDate"] = DateTime.Now;
            r["AccountType"] = (int)AccountType.Deposit;
            r["ClientId"] = client.Id;

            BankContext.AccountTable.Rows.Add(r);
            SaveAccount();

        }
    }
}
