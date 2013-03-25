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

        public static readonly FilePath SMUAudioPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "Rentit21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Audio" + Path.DirectorySeparatorChar);

        public static readonly FilePath SMULogPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "Rentit21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Log" + Path.DirectorySeparatorChar);

        public static readonly FilePath SMUPdfPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "Rentit21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "PDF" + Path.DirectorySeparatorChar);

        public static readonly FilePath ITUTrackPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "Rentit21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "Tracks" + Path.DirectorySeparatorChar);

        public static readonly FilePath ITULogPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "Rentit21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "Log" + Path.DirectorySeparatorChar);

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