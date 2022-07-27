using System;
using System.IO;
using System.Text;

namespace Common
{
    public class BankContext
    {
        public static void Log(string message)
        {
            using (StreamWriter sw = new StreamWriter("Logs.txt", true, Encoding.Default))
            {
                sw.WriteLine($"{DateTime.Now} || {message} || Менеджер : {Environment.UserName}");
            }
        }

    }
}
