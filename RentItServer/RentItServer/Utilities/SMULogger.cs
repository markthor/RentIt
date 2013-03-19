using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using RentItServer.SMU;

namespace RentItServer.Utilities
{
    public class Logger
    {
        /// <summary>
        /// The _entry lock. Used when the AddEntry method is called
        /// </summary>
        private static readonly object _entryLock = new object();

        /// <summary>
        /// The _task collection. Contains pending entries
        /// </summary>
        private readonly BlockingCollection<string> _taskCollection = new BlockingCollection<string>(new ConcurrentQueue<string>());

        /// <summary>
        /// The absolute path
        /// </summary>
        private readonly string AbsolutePath = "C:" + Path.DirectorySeparatorChar + 
            "Users" + Path.DirectorySeparatorChar + 
            "Rentit21" + Path.DirectorySeparatorChar +
            "Documents" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Logs" + Path.DirectorySeparatorChar + 
            "SMUlog.txt";

        /// <summary>
        /// Initializes a new instance of the Logger class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Full must not target a directory. absolutePath =  + absolutePath</exception>
        public Logger(string absolutePath, SMUController.LogEvent entryEvent)
        {
            if (File.Exists(AbsolutePath) == false)
            {
                File.Create(AbsolutePath);
            }

            entryEvent += new SMUController.LogEvent(AddEntry);
            // Logging thread. Used in order to support asyncrhonous writing of entries
            new Task(() =>
            {
                string logEntry;
                while (true)
                {
                    logEntry = _taskCollection.Take();
                    File.AppendAllText(AbsolutePath, logEntry);
                }
            }).Start();
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        /// <param name="entry">The entry to be appended to the log file.</param>
        /// <exception cref="System.ArgumentNullException">Log argument was null</exception>
        /// <exception cref="System.ArgumentException">Log argument was empty</exception>
        private void AddEntry(object sender, string entry)
        {
            if (entry == null) throw new ArgumentNullException("entry");
            if (entry.Equals("")) throw new ArgumentException("entry argument was empty");

            lock (_entryLock)
            {
                string timeStamp = "[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "]    ";
                _taskCollection.Add(timeStamp + entry);
            }
        }
    }
}
