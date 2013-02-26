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

        private String _root = "<Drive letter>:" + Path.DirectorySeparatorChar
                                + "Folder1" + Path.DirectorySeparatorChar
                                + "Folder2" + Path.DirectorySeparatorChar;

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

        public void Write(String relativePath, MemoryStream track)
        {
            relativePath = relativePath.Replace("\\", Path.DirectorySeparatorChar.ToString());
            relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            String fullPath = _root + relativePath;
            if (fullPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                if (Directory.Exists(fullPath) == false)
                {
                    Directory.CreateDirectory(fullPath);
                }
            }
            else
            {
                FileStream fs = File.OpenWrite(fullPath);
                track.CopyTo(fs);
                fs.Flush();
                fs.Close();
            }
        }
    }
}