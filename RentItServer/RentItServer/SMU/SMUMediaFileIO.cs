using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace RentItServer.SMU
{
    public static class SMUMediaFileIO
    {
        public static byte[] ReadStream(MemoryStream stream)
        {
            int streamLength = (int)stream.Length;
            byte[] buffer = new byte[streamLength];
            stream.Read(buffer, 0, streamLength);
            return buffer;
        }
    }
}