using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RentItServer
{
    public class Logger
    {
        private readonly string _fullPath;

        public Logger(string fullPath)
        {
            fullPath = fullPath.Replace("\\", Path.DirectorySeparatorChar.ToString());
            fullPath = fullPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (fullPath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("Full must not target a directory. fullPath = " + fullPath);
            
            _fullPath = fullPath;

            if (File.Exists(_fullPath) == false)
            {
                File.Create(_fullPath);
            }
        }

        public void AddEntry(string log)
        {
            if (log == null) throw new ArgumentNullException("Log argument was null");
            if (log.Equals("")) throw new ArgumentException("Log argument was empty");
            string timeStamp = "["+DateTime.Now.ToString()+"] ";
            File.AppendAllText(_fullPath, timeStamp + log);
        }
    }
}
