using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using RentItServer;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class FileSystemHandler_Test
    {
        [TestMethod]
        public void FileSystemHandler_Write()
        {
            byte[] bytes = File.ReadAllBytes("D:"+Path.DirectorySeparatorChar+"Games"+Path.DirectorySeparatorChar+"billede.bmp");
            MemoryStream ms = new MemoryStream(bytes);
            FileSystemHandler.GetInstance().Write("BILLEDER.bmp",ms);
        }
    }
}
