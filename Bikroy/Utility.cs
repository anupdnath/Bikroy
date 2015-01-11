using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Bikroy
{
   public class Utility
    {

       public static void LogFile(String FileName, String Message)
       {
           if (Message != null && Message.Length > 0)
           {
               StreamWriter writer = File.AppendText(FileName);
               try
               {
                   String DateFormat = "dd-MMM-yyyy hh:mm:ss";
                   writer.WriteLine(String.Format("{0} : {1}", DateTime.Now.ToString(DateFormat), Message));
               }
               finally
               {
                   writer.Close();
                   writer.Dispose();
               }
           }
       }
        public static void ErrorLog(Exception ex, String ErrorData)
        {
            try
            {
                
                    Thread.BeginCriticalRegion();
                    String ErrorFilename = "Errors.txt";
                    Utility.LogFile(ErrorFilename, ErrorData);
                    Utility.LogFile(ErrorFilename, ex.Message);
                    Utility.LogFile(ErrorFilename, ex.StackTrace);
                    Thread.EndCriticalRegion();
                
            }
            catch { }
        }
        public static void ErrorLog(Exception ex, String ErrorData, String FileName)
        {
            try
            {
                Thread.BeginCriticalRegion();
                String ErrorFilename = FileName;
                Utility.LogFile(ErrorFilename, ErrorData);
                Utility.LogFile(ErrorFilename, ex.Message);
                Utility.LogFile(ErrorFilename, ex.StackTrace);
                Thread.EndCriticalRegion();
            }
            catch { }
        }
    }
}
