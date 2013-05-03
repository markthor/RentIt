using System;
using System.IO;

namespace RentItServer.Utilities
{
    /// <summary>
    /// This class is used to communicate with the filesystem of the server.
    /// </summary>
    public class FileSystemDao
    {
        //Singleton instance of the class
        private static FileSystemDao _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private FileSystemDao() { }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static FileSystemDao GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FileSystemDao();
            }
            return _instance;
        }

        /// <summary>
        /// Writes the specified stream to a file at the path relative to the root directory.
        /// </summary>
        /// <param name="path">The path to the directory in which the file should be placed</param>
        /// <param name="filename">The name of the file</param>
        /// <param name="memoryStream">The stream containing the content of the file</param>
        /// <exception cref="System.ArgumentNullException">
        /// Path, filename or memoryStream was null
        /// </exception>
        public void WriteFile(FilePath path, string filename, MemoryStream memoryStream)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (filename == null) throw new ArgumentNullException("filename");
            if (memoryStream == null) throw new ArgumentNullException("memoryStream");

            //Full path to the file
            string fullPath = string.Concat(path.GetPath(), filename);

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
        /// Reads the file at the specified path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>
        ///   <see cref="MemoryStream" /> containing the contents of the file
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Relative path was null</exception>
        /// <exception cref="System.ArgumentException">Relative path must target a file
        /// or
        /// Relative path must target a file. Relative path =  + filename</exception>
        public MemoryStream ReadFile(string absolutePath)
        {
            if (absolutePath == null) throw new ArgumentNullException("absolutePath");
            if (absolutePath.Equals("")) throw new ArgumentException("Absolute path must target a file");
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("Absolute path must target a file. Absolute path = " + absolutePath);

            FileStream fs = File.OpenRead(absolutePath);
            MemoryStream ms = new MemoryStream();
            fs.CopyTo(ms);
            fs.Close();
            ms.Position = 0L;
            return ms;
        }

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <exception cref="System.ArgumentNullException">relativePath</exception>
        /// <exception cref="System.ArgumentException">Relative path must target a file
        /// or
        /// Relative path must target a file. Relative path =  + relativePath</exception>
        /// <exception cref="System.Exception">File with path [+fullPath+] was not deleted.</exception>
        public void DeleteFile(string absolutePath)
        {
            if (absolutePath == null) throw new ArgumentNullException("absolutePath");
            if (absolutePath.Equals("")) throw new ArgumentException("absolutePath path must target a file");
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("absolutePath path must target a file. AbsolutePath path = " + absolutePath);

            File.Delete(absolutePath);
            if (File.Exists(absolutePath)) throw new Exception("File with path [" + absolutePath + "] was not deleted.");
        }

        /// <summary>
        /// Checks if the file with the given absolute path exists
        /// </summary>
        /// <param name="absolutePath">The absolute path of the file</param>
        /// <returns>Whether the file exists or not</returns>
        public bool Exists(string absolutePath)
        {
            if (absolutePath == null) throw new ArgumentNullException("absolutePath");
            if (absolutePath.Equals("")) throw new ArgumentException("absolutePath path must target a file");
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("absolutePath path must target a file. AbsolutePath path = " + absolutePath);

            return File.Exists(absolutePath);
        }
    }
}