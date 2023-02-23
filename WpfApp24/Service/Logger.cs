using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoBD.Service
{
    internal class Logger
    {
        private readonly String filename;
        public Logger(String filename)
        {
            this.filename = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,filename);
        }
        public void Log(String message, String level = "INFO")
        {
            File.AppendAllText(filename, $"{level} - {message}\r\n");
        }
    }
}
