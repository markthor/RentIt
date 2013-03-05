using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace RentItServer_v1
{
    public class FileSystemHandler
    {
        //Singleton instance of the class
        private static FileSystemHandler _instance;

        //TODO: This should be defined through argument in constructor in future
        private String _root = "<Drive letter>:" + Path.DirectorySeparatorChar
                                + "Folder1" + Path.DirectorySeparatorChar
                                + "Folder2" + Path.DirectorySeparatorChar;

        /// <summary>
        /// The log
        /// </summary>
        private readonly Logger _log = Logger.GetInstance();

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private FileSystemHandler()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static FileSystemHandler GetInstance()
        {
            return _instance ?? (_instance = new FileSystemHandler());
        }

        /// <summary>
        /// Writes the specified trackStream to a file at the path relative to the root directory.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="trackStream">The track stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Relative path was null
        /// or
        /// MemoryStream argument was null
        /// </exception>
        public void Write(string relativePath, MemoryStream trackStream)
        {
            if (relativePath == null) throw new ArgumentNullException("relativePath");
            if (trackStream == null) throw new ArgumentNullException("trackStream");
            
            string fullPath = ProcessPath(relativePath);            
            if (fullPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {   // The path specifies a directory
                if (Directory.Exists(fullPath) == false)
                {   // Create the directory
                    Directory.CreateDirectory(fullPath);
                }
            }
            else
            {
                FileStream fs = File.OpenWrite(fullPath);
                trackStream.CopyTo(fs);
                trackStream.Close();
                fs.Flush();
                fs.Close();
            }
        }

        /// <summary>
        /// Reads the file at the specified path relative to the root directory.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns> <see cref="MemoryStream"/> containing the contents of the file</returns>
        /// <exception cref="System.ArgumentNullException">Relative path was null</exception>
        /// <exception cref="System.ArgumentException">
        /// Relative path must target a file
        /// or
        /// Relative path must target a file. Relative path =  + relativePath
        /// </exception>
        public MemoryStream Read(string relativePath)
        {
            if (relativePath == null) throw new ArgumentNullException("relativePath");
            if (relativePath.Equals("")) throw new ArgumentException("Relative path must target a file");
            
            string fullPath = ProcessPath(relativePath);
            if (relativePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("Relative path must target a file. Relative path = " + relativePath);
            
            try
            {
                FileStream fs = File.OpenRead(fullPath);
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);
                fs.Close();
                return ms;
            }
            catch(Exception e)
            {
                _log.AddEntry( @"Exception thrown: " + e + ". " +
                                "FileSystemHandler state: _root = " + _root + ". " +
                                "Local variables: fullPath = " + fullPath +".");
                throw;
            }
        }

        /// <summary>
        /// Processes the path.
        /// </summary>
        /// <param name="relativePath">The relative path to a file or directory.</param>
        /// <returns>The absolute path to file or directory.</returns>
        private string ProcessPath(string relativePath)
        {
            relativePath = relativePath.Replace("\\", Path.DirectorySeparatorChar.ToString());
            relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            return _root + relativePath;
        }
    }
}