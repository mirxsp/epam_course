using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace lab2.Model.Printers
{
    public class FilePrinter : Interface.IPrinter,IDisposable
    {
        private readonly StreamWriter writer;
        private bool disposedValue;

        public FilePrinter(string filename,bool append)
        {
            writer = new StreamWriter(filename, append);
        }

        public void Print(string String)
        {
            
            writer.WriteLine(String);
            writer.Flush();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    writer.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
