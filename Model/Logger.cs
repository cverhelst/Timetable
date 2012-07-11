using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Model
{
    public class Logger
    {
        public static string FILENAME = "tables.log";

        public static void Log(string output)
        {
            // Create a writer and open the file:
            StreamWriter log;

            if (!File.Exists(FILENAME))
            {
                log = new StreamWriter(FILENAME);
            }
            else
            {
                log = File.AppendText(FILENAME);
            }

            // Write to the file:
            log.WriteLine(DateTime.Now);
            log.WriteLine(output);
            log.WriteLine();

            // Close the stream:
            log.Close();
        }

        public static void Reset()
        {
            File.Delete(FILENAME);
        }
    }
}
