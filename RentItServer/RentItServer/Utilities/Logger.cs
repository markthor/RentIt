using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

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
        /// The _task collection. Contains pending entries
        /// </summary>
        private readonly BlockingCollection<string> _taskCollection = new BlockingCollection<string>(new ConcurrentQueue<string>());

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

        /// <summary>
        /// Initializes a new instance of the Logger class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Full must not target a directory. absolutePath =  + absolutePath</exception>
        public Logger(string absolutePath, ref EventHandler handler)
        {
            String directory = absolutePath.Substring(0, absolutePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            Directory.CreateDirectory(directory);
            if (File.Exists(absolutePath) == false)
            {
                File.Create(absolutePath);
            }
            this._absolutePath = absolutePath;
            handler += AddEntry;
            // Logging thread. Used in order to support asyncrhonous writing of entries
            new Task(() =>
            {
                string logEntry;
                while (true)
                {
                    logEntry = _taskCollection.Take();
                    File.AppendAllText(absolutePath, logEntry);
                }
            }).Start();
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArguments">The event arguments.</param>
        /// <exception cref="System.ArgumentNullException">Log argument was null</exception>
        private void AddEntry(object sender, EventArgs eventArguments)
        {
            if (eventArguments == null) throw new ArgumentNullException("eventArguments");

            RentItEventArgs args = eventArguments as RentItEventArgs;
            if (args == null) return;

            lock (_entryLock)
            {
                string timeStamp = "[" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + "] ";
                //File.AppendAllText(_absolutePath, timeStamp + args.Entry + Environment.NewLine); 
                _taskCollection.Add(timeStamp + args.Entry + Environment.NewLine);
            }
        }
    }
}