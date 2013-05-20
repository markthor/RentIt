using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

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
            using (FileStream fs = File.OpenWrite(fullPath))
            {
                //Write the content and close the resources
                //memoryStream.CopyTo(fs);
                memoryStream.WriteTo(fs);
                memoryStream.Close();
            }
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
            if (absolutePath == null) return false;
            if (absolutePath.Equals("")) return false;
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar.ToString())) return false;

            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Writes the specified string to a file at the absolute file path. Can overwrite.
        /// </summary>
        /// <param name="path">The path to the directory in which the file should be placed, including the file name and file ending</param>
        /// <param name="content">The string containing the content of the file</param>
        /// <exception cref="System.ArgumentNullException">
        /// Path was null
        /// </exception>
        public void WriteFile(string content, string absolutePath)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (absolutePath == null) throw new ArgumentNullException("absolutePath");

            FileStream fs = File.OpenWrite(absolutePath);
            Byte[] bytes = GetBytes(content);
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
            fs.Close();
        }

        private byte[] GetBytes(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Writes the list of tracks' path to a .m3u file on the system
        /// </summary>
        /// <param name="absolutePath">Where the .m3u file should be saved. Including the file name and file ending</param>
        /// <param name="playlist">The list of tracks which should be in the file</param>
        public void WriteM3UPlaylistFile(string absolutePath, List<Track> playlist)
        {
            if (playlist == null) throw new ArgumentNullException("playlist");
            if (playlist.Count == 0) throw new ArgumentException("empty playlist");

            //Check if the file exists and create the file if it doesn't
            FileStream fs = null;
            if (!File.Exists(absolutePath))
            {
                using (fs = File.Create(absolutePath))
                {

                }
            }

            if (File.Exists(absolutePath)) // Make sure the file exists
            {
                //Open a writer to the file
                using (StreamWriter sw = new StreamWriter(absolutePath))
                {
                    //Loop through all tracks and add their path to the file
                    foreach (Track t in playlist)
                    {
                        sw.WriteLine(t.Path);
                    }
                    //Make sure everything is flushed to the file
                    sw.Flush();
                }
            }
        }
    }
}