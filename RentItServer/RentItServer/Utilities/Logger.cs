using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

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
        /// Initializes a new instance of the Logger class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Full must not target a directory. absolutePath =  + absolutePath</exception>
        public Logger(string absolutePath, ref EventHandler handler)
        {
            String directory = absolutePath.Substring(0, absolutePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            Directory.CreateDirectory(directory);
            //if (File.Exists(absolutePath))
            //{
                File.Create(absolutePath);
            //}

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
                string timeStamp = "[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "] ";
                _taskCollection.Add(timeStamp + args.Entry + Environment.NewLine);
            }
        }
    }
}
