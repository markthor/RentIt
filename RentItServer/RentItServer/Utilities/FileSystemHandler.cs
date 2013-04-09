using System;
using System.IO;

namespace RentItServer.Utilities
{
    /// <summary>
    /// This class is used to communicate with the filesystem of the server.
    /// </summary>
    public class FileSystemHandler
    {
        //Singleton instance of the class
        private static FileSystemHandler _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private FileSystemHandler() { }

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
        /// Reads the file at the specified path with the given filename.
        /// </summary>
        /// <param name="path">The path where the file is placed</param>
        /// <param name="filename">The name of the file</param>
        /// <returns>The MemoryStream containing the contents of the file</returns>
        /// <exception cref="System.ArgumentNullException">filename was null</exception>
        /// <exception cref="System.ArgumentException">filename must be a filename.</exception>
        public MemoryStream ReadFile(FilePath path, string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (filename.Equals("")) throw new ArgumentException("filename must be a filename.");
            if (filename.EndsWith(Path.DirectorySeparatorChar.ToString())) throw new ArgumentException("filename must be a filename. filename = " + filename);
            
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
        /// Deletes the file at the given path
        /// </summary>
        /// <param name="absolutePath">The absolute path to the file</param>
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