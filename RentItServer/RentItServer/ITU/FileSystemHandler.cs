using System;
using System.IO;

namespace RentItServer.ITU
{
    public class FileSystemHandler
    {
        //The target folder for write and read operations.
        private string _root;

        /// <summary>
        /// The log
        /// </summary>
        private readonly Logger _log = Logger.GetInstance();

        /// <summary>
        /// Constructs a FileSystemHandler with the specific file path.
        /// </summary>
        /// <param name="path">The path of the folder to contain files</param>
        public FileSystemHandler(string path)
        {
            _root = CorrectSeperator(path);
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
        public void WriteFile(string relativePath, MemoryStream trackStream)
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
                _log.AddEntry( @"Exception thrown [" + e + "]. " +
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
    }
}