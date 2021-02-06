using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Task4.Processors.BL;

namespace Task4.ConsoleClient
{
    class Program
    {
        static TextWriter writer;

        static void Main(string[] args)
        {
            FileProcessor processor;
            string targetFolder = ConfigurationManager.AppSettings["targetFolder"];
            string logFile = ConfigurationManager.AppSettings["logFile"];
            bool.TryParse(ConfigurationManager.AppSettings["consoleLogging"], out bool consoleLogging);
            if (logFile != null && !consoleLogging)
            {
                try
                {
                    writer = new StreamWriter(logFile);
                }
                catch(IOException)
                {
                    //
                }
            }
            if(consoleLogging)
            {
                writer = Console.Out;
            }
            ServiceController serviceController = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == "ServiceClient");
            if(serviceController !=null && serviceController.Status==ServiceControllerStatus.Running)
            {
                if(writer!=null)
                {
                    Log("ServiceClient is running. ConsoleClient turns off.");
                }
                return;
            }
            if (string.IsNullOrWhiteSpace(targetFolder))
            {
                if (writer != null)
                {
                    Log("Invalid target folder");
                }
                return;
            }
            processor = new FileProcessor(targetFolder);
            if (writer != null)
            {
                processor.Log += Log;
            }
            while (true)
            {
                Console.WriteLine(
                    "Press 1 to Scan\n" +
                    "Press any other key to exit");
                if (Console.ReadKey().Key == ConsoleKey.D1)
                {
                    Console.WriteLine();
                    processor.ScanForExistingFiles();
                }else
                {
                    break;
                }
            }
            processor.Dispose();
        }

        static void Log(string s)
        {
            writer.WriteLine(s);
            writer.Flush();
        }
    }
}
