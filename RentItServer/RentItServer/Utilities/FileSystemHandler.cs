using System;
using System.IO;
using System.ServiceModel;

namespace RentItServer.Utilities
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
        /// <param name="path">The path to the directory in which the file should be placed</param>
        /// <param name="filename">The name of the file</param>
        /// <param name="memoryStream">The track stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Relative path was null
        /// or
        /// MemoryStream argument was null
        /// </exception>
        public void WriteFile(FilePath path, string filename, MemoryStream memoryStream)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (filename == null) throw new ArgumentNullException("filename");
            if (memoryStream == null) throw new ArgumentNullException("memoryStream");

            //Full path to the file
            string fullPath = path.GetPath() + filename;
            
            //Create the directory
            Directory.CreateDirectory(path.GetPath());
            //Open the file to write to it
            FileStream fs = File.OpenWrite(fullPath);
            //Write the content and close the resources
            memoryStream.CopyTo(fs);
            memoryStream.Close();
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// Reads the file at the specified path relative to the root directory.
        /// </summary>
        /// <param name="path">The path where the file should be placed</param>
        /// <param name="filename">The name of the file</param>
        /// <returns> <see cref="MemoryStream"/> containing the contents of the file</returns>
        /// <exception cref="System.ArgumentNullException">Relative path was null</exception>
        /// <exception cref="System.ArgumentException">
        /// Relative path must target a file
        /// or
        /// Relative path must target a file. Relative path =  + filename
        /// </exception>
        public MemoryStream ReadFile(FilePath path, string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (filename.Equals("")) throw new ArgumentException("Relative path must target a file");
            if (filename.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("Relative path must target a file. Relative path = " + filename);
            
            //Full path to the file
            string fullPath = path.GetPath() + filename;
            FileStream fs = File.OpenRead(fullPath);
            MemoryStream ms = new MemoryStream();
            fs.CopyTo(ms);
            fs.Close();
            ms.Position = 0L;
            return ms;
        }

        /// <summary>
        /// Deletes the file at the specified path+relativepath.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <exception cref="System.ArgumentNullException">relativePath</exception>
        /// <exception cref="System.ArgumentException">
        /// Relative path must target a file
        /// or
        /// Relative path must target a file. Relative path =  + relativePath
        /// </exception>
        /// <exception cref="System.Exception">File with path [+fullPath+] was not deleted.</exception>
        public void DeleteFile(string absolutePath)
        {
            if (absolutePath == null) throw new ArgumentNullException("absolutePath");
            if (absolutePath.Equals("")) throw new ArgumentException("absolutePath path must target a file");
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("absolutePath path must target a file. AbsolutePath path = " + absolutePath);

            File.Delete(absolutePath);
            if (File.Exists(absolutePath) == true) throw new Exception("File with path [" + absolutePath + "] was not deleted.");
        }

        public bool Exists(string absolutePath)
        {
            if (absolutePath == null) throw new ArgumentNullException("absolutePath");
            if (absolutePath.Equals("")) throw new ArgumentException("absolutePath path must target a file");
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("absolutePath path must target a file. AbsolutePath path = " + absolutePath);

            return File.Exists(absolutePath);
        }

        internal static byte[] LoadTrackBytes(string p)
        {
            throw new NotImplementedException();
        }
    }
}