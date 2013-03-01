using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RentItServer
{
    public class Logger
    {
        /// <summary>
        /// The absolute path to the log file
        /// </summary>
        private readonly string _absolutePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="absolutePath">The absolute path to the log file. If a file does not exist at the specified location it will be created.</param>
        /// <exception cref="System.ArgumentException">Full must not target a directory. absolutePath =  + absolutePath</exception>
        public Logger(string absolutePath)
        {
            absolutePath = absolutePath.Replace("\\", Path.DirectorySeparatorChar.ToString());
            absolutePath = absolutePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("Full must not target a directory. absolutePath = " + absolutePath);
            
            _absolutePath = absolutePath;

            if (File.Exists(_absolutePath) == false)
            {
                File.Create(_absolutePath);
            }
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        /// <param name="entry">The entry to be appended to the log file.</param>
        /// <exception cref="System.ArgumentNullException">Log argument was null</exception>
        /// <exception cref="System.ArgumentException">Log argument was empty</exception>
        public void AddEntry(string entry)
        {
            if (entry == null) throw new ArgumentNullException("Log argument was null");
            if (entry.Equals("")) throw new ArgumentException("Log argument was empty");
            string timeStamp = "["+DateTime.Now.ToString()+"] ";
            File.AppendAllText(_absolutePath, timeStamp + entry);
        }
    }
}
