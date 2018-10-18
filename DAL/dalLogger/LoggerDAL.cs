using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL.dalLogger
{
    public class LoggerDAL
    {
        //declaring string to assign it when needed
        public static string ErrorLogPath;

        //logging regular errors
        public static void ErrorLog(Exception ex)
        {
            using (StreamWriter updateErrorLog = new StreamWriter(ErrorLogPath, true))
            {
                updateErrorLog.WriteLine(new String('-', 5));

                updateErrorLog.WriteLine("Time: {0}\r\nError:{1}", DateTime.Now.ToString(), ex);
                updateErrorLog.Close();
                updateErrorLog.Dispose();
            }
        }

        //logging SQL errors
        public static void SqlErrorLog(Exception sqlEx)
        {
            using (StreamWriter updateErrorLog = new StreamWriter(ErrorLogPath, true))
            {
                updateErrorLog.WriteLine(new String('-', 5));

                updateErrorLog.WriteLine("Time: {0}\r\nError:{1}", DateTime.Now.ToString(), sqlEx);
                updateErrorLog.Close();
                updateErrorLog.Dispose();
            }
        }
    }
}
