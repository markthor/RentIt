using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace RentItServer
{
    public class Logger
    {
        /// <summary>
        /// The absolute path
        /// </summary>
        private const string AbsolutePath = "";

        /// <summary>
        /// The logger
        /// </summary>
        private static Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="absolutePath">The absolute path to the log file. If a file does not exist at the specified location it will be created.</param>
        /// <exception cref="System.ArgumentException">Full must not target a directory. absolutePath =  + absolutePath</exception>
        private Logger()
        {
            if (File.Exists(AbsolutePath) == false)
            {
                File.Create(AbsolutePath);
            }
        }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        /// <returns></returns>
        public static Logger GetInstance()
        {
            return _logger ?? (_logger = new Logger());
        }

        /// <summary>
        /// Adds an entry to the log.
        /// </summary>
        /// <param name="entry">The entry to be appended to the log file.</param>
        /// <exception cref="System.ArgumentNullException">Log argument was null</exception>
        /// <exception cref="System.ArgumentException">Log argument was empty</exception>
        public void AddEntry(string entry)
        {
            if (entry == null) throw new ArgumentNullException("entry argument was null");
            if (entry.Equals("")) throw new ArgumentException("entry argument was empty");
            string timeStamp = "["+DateTime.Now.ToString(CultureInfo.InvariantCulture)+"] ";
            File.AppendAllText(AbsolutePath, timeStamp + entry);
        }
    }
}
