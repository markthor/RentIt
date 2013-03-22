using System;
using System.IO;
using RentItServer.Utilities;
namespace RentItServer.ITU
{
    public class FileSystemHandler
    {
        //The target folder for write and read operations.
        //private string _root;

        /// <summary>
        /// The log
        /// </summary>
        //private readonly Logger _log = Logger.GetInstance();

        //Singleton instance of the class
        private static FileSystemHandler _instance;

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
        public void WriteFile(FilePath path, string relativePath, MemoryStream trackStream)
        {
            if (relativePath == null) throw new ArgumentNullException("relativePath");
            if (trackStream == null) throw new ArgumentNullException("trackStream");

            string fullPath = path + relativePath; //ProcessPath(path + relativePath);            
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
        public MemoryStream Read(FilePath path, string relativePath)
        {
            if (relativePath == null) throw new ArgumentNullException("relativePath");
            if (relativePath.Equals("")) throw new ArgumentException("Relative path must target a file");

            string fullPath = path + relativePath; //ProcessPath(path + relativePath);
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
                throw;
            }
        }

        /*
        /// <summary>
        /// Processes the path.
        /// </summary>
        /// <param name="relativePath">The relative path to a file or directory.</param>
        /// <returns>The absolute path to file or directory.</returns>
        private string ProcessPath(string relativePath)
        {
            relativePath = CorrectSeperator(relativePath);
            return _root + relativePath;
        }

        /// <summary>
        /// Corrects the folder seperators
        /// </summary>
        /// <param name="path">The path to correct</param>
        /// <returns>The corrected path</returns>
        private string CorrectSeperator(string path)
        {
            path = path.Replace("\\", Path.DirectorySeparatorChar.ToString());
            path = path.Replace("/", Path.DirectorySeparatorChar.ToString());
            return path;
        }
        */

        internal static byte[] LoadTrackBytes(string p)
        {
            throw new NotImplementedException();
        }
    }
}