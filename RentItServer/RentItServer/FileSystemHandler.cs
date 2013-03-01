using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace RentItServer
{
    public class FileSystemHandler
    {
        //Singleton instance of the class
        public static FileSystemHandler _instance;

        //TODO: This should be defined through argument in constructor in future
        private String _root = "<Drive letter>:" + Path.DirectorySeparatorChar
                                + "Folder1" + Path.DirectorySeparatorChar
                                + "Folder2" + Path.DirectorySeparatorChar;

        private readonly Logger _log = new Logger("ABSOLUTE PATH TO LOG FILE");

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
            if (_instance == null)
            {
                _instance = new FileSystemHandler();
            }
            return _instance;
        }

        public void Write(string relativePath, MemoryStream trackStream)
        {
            if (relativePath == null) throw new ArgumentNullException("Relative path was null");
            if (trackStream == null) throw new ArgumentNullException("MemoryStream argument was null");
            
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

        public MemoryStream Read(string relativePath)
        {
            if (relativePath == null) throw new ArgumentNullException("Relative path was null");
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
                _log.AddEntry(
                @"Exception thrown: " + e.ToString() + ". " +
                 "FileSystemHandler state: _root = " + _root + ". " + 
                 "Local variables: fullPath = " + fullPath);
                throw e;
            }
        }

        private string ProcessPath(string relativePath)
        {
            relativePath = relativePath.Replace("\\", Path.DirectorySeparatorChar.ToString());
            relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            return _root + relativePath;
        }
    }
}