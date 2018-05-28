using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Utility
{
    public class MessageLogger
    {
        public static void LogErroneousMessage(string message, string reason)
        {
            string path = System.IO.Path.GetTempPath() + "errorlog.txt";
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Erroneous Messages");
                        sw.WriteLine("*****************");
                        sw.WriteLine();
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("START MESSAGE");
                    sw.WriteLine(message);
                    sw.WriteLine("END MESSAGE");
                    sw.WriteLine("Reason: " + reason);
                    sw.WriteLine();
                    sw.WriteLine();
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public static void LogRejectedMessage(string message, string reason)
        {
            string path = System.IO.Path.GetTempPath() + "rejectlog.txt";
            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Rejected Messages");
                        sw.WriteLine("*****************");
                        sw.WriteLine();
                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("START MESSAGE");
                    sw.WriteLine(message);
                    sw.WriteLine("END MESSAGE");
                    sw.WriteLine("Reason: " + reason);
                    sw.WriteLine();
                    sw.WriteLine();
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
