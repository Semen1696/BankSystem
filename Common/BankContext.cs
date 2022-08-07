using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Common
{
    public class BankContext
    {
        public static SqlConnection con;
        public static DataTable ClientTable;
        public static DataTable AccountTable;
        public static void Log(string message)
        {

            var adapter = new SqlDataAdapter
            {
                InsertCommand = new SqlCommand(@"INSERT INTO Logs (Text,  CreateDate) 
                                              VALUES (@Text, @CreateDate); 
                                              SET @Id = @@IDENTITY;", con),
            };
            adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");
            adapter.InsertCommand.Parameters.Add("@Text", SqlDbType.NVarChar, int.MaxValue, "Text");
            adapter.InsertCommand.Parameters.Add("@CreateDate", SqlDbType.DateTime, 20, "CreateDate");

            var dt = new DataTable();
            string sql = $@"SELECT * FROM Logs";
            adapter.SelectCommand = new SqlCommand(sql, con);
            adapter.Fill(dt);

            DataRow r = dt.NewRow();

            r["Text"] = message;
            r["CreateDate"] = DateTime.Now;

            dt.Rows.Add(r);
            adapter.Update(dt);

        }

        public static void Connect()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "Bank"
            };
            con = new SqlConnection(connectionStringBuilder.ConnectionString);
        }

    }
}
