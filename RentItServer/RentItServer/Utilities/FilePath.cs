using System.IO;

namespace RentItServer.Utilities
{
    /// <summary>
    /// Class containing constants for filepaths to various files related to the ITU & SMU service
    /// </summary>
    public sealed class FilePath
    {
        /// <summary>
        /// The path
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The channel config files for ezstream
        /// </summary>
        public static readonly FilePath ITUChannelConfigPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "ChannelConfig" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The executable ezstream files
        /// </summary>
        public static readonly FilePath ITUEzStreamPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "EzStream" + Path.DirectorySeparatorChar +
            "ezstream.exe");

        /// <summary>
        /// The M3u files refered in the config files.
        /// </summary>
        public static readonly FilePath ITUM3uPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "M3u" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The SMU audio path
        /// </summary>
        public static readonly FilePath SMUAudioPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Audio" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The SMU log path
        /// </summary>
        public static readonly FilePath SMULogPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Log" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The SMU PDF path
        /// </summary>
        public static readonly FilePath SMUPdfPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "PDF" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The SMU image path
        /// </summary>
        public static readonly FilePath SMUImagePath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Images" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The ITU track path
        /// </summary>
        public static readonly FilePath ITUTrackPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "Tracks" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The ITU temp path
        /// </summary>
        public static readonly FilePath ITUTempPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "Temp" + Path.DirectorySeparatorChar);

        /// <summary>
        /// The ITU log path
        /// </summary>
        public static readonly FilePath ITULogPath = new FilePath(
            "C:" + Path.DirectorySeparatorChar +
            "RentItServices" + Path.DirectorySeparatorChar +
            "RentIt21Files" + Path.DirectorySeparatorChar +
            "ITU" + Path.DirectorySeparatorChar +
            "Log" + Path.DirectorySeparatorChar);

        /// <summary>
        /// Prevents a default instance of the <see cref="FilePath"/> class from being created.
        /// </summary>
        /// <param name="path">The path.</param>
        private FilePath(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            return _path;
        }

        public override string ToString()
        {
            return _path;
        }
    }
}