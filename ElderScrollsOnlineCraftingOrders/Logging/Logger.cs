using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Reflection;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace ElderScrollsOnlineCraftingOrders.Logging
{
    public static class Logger
    {
        //declaring string to reassign
        public static string errorLogPath;

        //logging regular errors
        public static void ErrorLog(Exception ex)
        {
            using (StreamWriter updateErrorLog = new StreamWriter(errorLogPath, true))
            {
                updateErrorLog.WriteLine(new String('-', 5));

                updateErrorLog.WriteLine("Time: {0}\r\nError:{1}", DateTime.Now.ToString(), ex);
            }
        }

        //logging SQL errors
        public static void SqlErrorLog(Exception sqlEx)
        {
            using (StreamWriter updateErrorLog = new StreamWriter(errorLogPath, true))
            {
                updateErrorLog.WriteLine(new String('-', 5));

                updateErrorLog.WriteLine("Time: {0}\r\nError:{1}", DateTime.Now.ToString(), sqlEx);
            }
        }
    }
}