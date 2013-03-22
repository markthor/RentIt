using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace RentItServer.Utilities
{
    public sealed class FilePath
    {
        private string path;

        public static readonly FilePath SMUFilePath = new FilePath("C:" + Path.DirectorySeparatorChar +
            "Users" + Path.DirectorySeparatorChar +
            "Rentit21" + Path.DirectorySeparatorChar +
            "Documents" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Files");

        public static readonly FilePath ITUTrackPath = new FilePath("C:" + Path.DirectorySeparatorChar +
            "Users" + Path.DirectorySeparatorChar +
            "Rentit21" + Path.DirectorySeparatorChar +
            "Documents" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "Tracks");

        private FilePath(string path)
        {
            this.path = path;
        }

        public string GetPath()
        {
            return path;
        }
    }
}