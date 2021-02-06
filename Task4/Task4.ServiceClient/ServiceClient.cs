using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using Task4.Processors.BL;

namespace Task4.ServiceClient
{
    public partial class ServiceClient : ServiceBase
    {
        FileProcessor processor;
        StreamWriter writer;

        public ServiceClient()
        {
            string targetFolder = ConfigurationManager.AppSettings["targetFolder"];
            string logFile = ConfigurationManager.AppSettings["logFile"];
            bool.TryParse(ConfigurationManager.AppSettings["checkExistingFilesOnStart"], out bool checkExistingFilesOnStart);
            if (logFile != null)
            {
                try
                {
                    writer = new StreamWriter(logFile);
                }
                catch(IOException)
                {
                    //Block closing
                }
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
            if(writer!=null)
            {
                processor.Log += Log;
            }
            if (checkExistingFilesOnStart)
            {
                processor.ScanForExistingFiles();
            }
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if(System.Diagnostics.Process.GetProcessesByName("Task4.ConsoleClient").Length>0)
            {
                if(writer!=null)
                {
                    Log("ConsoleClient is already running. ServiceClient is turns off");
                    this.Stop();
                }
            }
            processor.RunBackgroundWatcher();
        }

        private void Log(string s)
        {
            writer.WriteLine(s);
            writer.Flush();
        }

        protected override void OnStop()
        {
            processor.StopBackgroundWatcher();
            writer.Dispose();
        }
    }
}
