using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SqlQueries
    {
        public static string InsertClient = @"INSERT INTO Clients (Name,  Surname,  Phone, Age, ClientTypeId) 
                                              VALUES (@Name, @Surname, @Phone, @Age, @ClientTypeId); 
                                              SET @Id = @@IDENTITY;";
        public static string UpdateClient = @"UPDATE Clients 
                                              SET Name = @Name, Surname = @Surname, Phone = @Phone, Age = @Age, ClientTypeId = @ClientTypeId
                                              WHERE Id = @Id";
        public static string DeleteClient = "DELETE FROM Clients WHERE Id = @Id";

        public static string InsertAccont = @"INSERT INTO Accounts (Number,  Amount,  CreateDate, AccountType, ClientId) 
                                              VALUES (@Number, @Amount, @CreateDate, @AccountType, @ClientId); 
                                              SET @Id = @@IDENTITY;";

        public static string UpdateAccount = @"UPDATE Accounts 
                                               SET Number = @Number, Amount = @Amount, CreateDate = @CreateDate, AccountType = @AccountType, ClientId = @ClientId
                                               WHERE Id = @Id";
        public static string DeleteAccount = "DELETE FROM Accounts WHERE Id = @Id";
    }
}
