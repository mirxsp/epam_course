using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Task4.Committers.BL;
using Task4.Converters.BL;
using Task4.Model;

namespace Task4.Processors.BL
{
    public class FileProcessor : IDisposable
    {
        public static bool CheckFilename(string name)
        {
            return Regex.IsMatch(name, @"\w+_([0-2][1-9]|3[0-1])(0[1-9]|1[0-2])\d{4}\.csv"); 
        }
        private FileSystemWatcher watcher;
        private bool disposedValue;
        public string TargetFolder { get; private set; }
        public bool IsWatcherActive { get; private set; }


        public event Action<string> Log;
        public event Action<string> LogDB;

        public FileProcessor(string targetFolder)
        {
            if (targetFolder.Length>0 && targetFolder[targetFolder.Length - 1] != '\\')
            {
                targetFolder += @"\";
            }
            TargetFolder = targetFolder;
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder + @"unprocessed");
            }
        }

        public void RunBackgroundWatcher()
        {
            if (!IsWatcherActive)
            {
                watcher = new FileSystemWatcher(TargetFolder + @"unprocessed\")
                {
                    EnableRaisingEvents = true,
                    Filter = "*.csv"
                };
                ThreadPool.SetMaxThreads(3, 3);
                watcher.Created += QueueFile;
                IsWatcherActive = true;
                Log?.Invoke("Watcher launched");
            }else
            {
                Log?.Invoke("Watcher is already running");
            }
        }

        public void StopBackgroundWatcher()
        {
            if (IsWatcherActive)
            {
                IsWatcherActive = false;
                watcher.Dispose();
                Log?.Invoke("Watcher is stopped");
            }else
            {
                Log?.Invoke("Watcher is already stopped");
            }
        }

        public void ScanForExistingFiles()
        {
            string[] filesToProcess = Directory.GetFiles(TargetFolder+@"unprocessed\", "*.csv");
            if(filesToProcess.Count()==0)
            {
                Log?.Invoke("There is no files to process");
                return;
            }else
            {
                Log?.Invoke("There is " + filesToProcess.Count() + " files to process");
            }
            foreach (string file in filesToProcess)
            {
                string[] fileSplit = file.Split('\\');
                FileSystemEventArgs args = new FileSystemEventArgs(WatcherChangeTypes.Created, TargetFolder+ @"unprocessed\", fileSplit[fileSplit.Length-1]);
                QueueFile(this, args);
            }
        }

        private void Process(FileSystemEventArgs args)
        {
            for(var count = 0;count<15;count++)
            { 
                if(!File.Exists(args.FullPath))
                {
                    Log?.Invoke(args.Name + " is missing");
                    break;
                }
                try
                {
                    Log?.Invoke(String.Format("Trying to process {0}. Attempt {1}/15",args.Name,count));
                    DataCommiter commiter;
                    try
                    {
                        commiter = new DataCommiter();
                    }
                    catch (Exception e)
                    {
                        Log?.Invoke("DB connection failed. " + e.Message);
                        break;
                    }
                    using (commiter)
                    {
                        commiter.Log += Log;
                        commiter.LogDB += LogDB;
                        Sale sale = null;
                        try
                        {
                            sale = CSVConverter.Convert(args.FullPath);
                        }
                        catch(FormatException e)
                        {
                            Log?.Invoke(e.Message);
                            break;
                        }
                        if (commiter.Push(sale))
                        {
                            Log?.Invoke("Deleting copy in processed folder");
                            Directory.CreateDirectory(TargetFolder + @"processed");
                            File.Delete(TargetFolder + @"processed\" + args.Name);
                            Thread.Sleep(2000);
                            File.Move(args.FullPath, TargetFolder + @"processed\" + args.Name);
                            Log?.Invoke(args.Name + " is processed");
                            Thread.Sleep(2000);
                            break;
                        }
                        else
                        {
                            Log?.Invoke(args.Name + " isn`t processed");
                        }
                    }
                }
                catch (IOException)
                {
                    continue;
                }
                
            }
        }

        private void QueueFile(object sender,FileSystemEventArgs args)
        {
            if (CheckFilename(args.Name))
            {
                ThreadPool.QueueUserWorkItem(x => Process(args));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsWatcherActive)
                    {
                        watcher.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        ~FileProcessor()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
