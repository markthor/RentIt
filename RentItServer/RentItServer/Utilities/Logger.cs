using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;

namespace RentItServer.Utilities
{
    /// <summary>
    /// Class for logging activites. It can write log entries to a log file located at the absollute path given in the constructor
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The _entry lock. Used when the AddEntry method is called
        /// </summary>
        private static readonly object _entryLock = new object();
        
        /// <summary>
        /// The _absolute path
        /// </summary>
        private readonly string _absolutePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        public Logger(string absolutePath)
        {
            _absolutePath = absolutePath;
            String directory = absolutePath.Substring(0, absolutePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            Directory.CreateDirectory(directory);
            if (File.Exists(absolutePath) == false)
            {
                File.Create(absolutePath);
            }
        }

        /// <summary>
        /// Adds the entry to the log file.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void AddEntry(string entry)
        {
            lock (_entryLock)
            {
                string timeStamp = "[" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + "] ";
                File.AppendAllText(_absolutePath, timeStamp + entry + Environment.NewLine);
            }
        }
    }
}