using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using RentItServer;
using RentItServer.ITU;

namespace RentItServer_UnitTests
{
    [TestClass]
    public class FileSystemHandler_Test
    {
        [TestMethod]
        public void FileSystemHandler_Write()
        {
            FileSystemHandler fs = new FileSystemHandler("C:" + Path.DirectorySeparatorChar +
            "Users" + Path.DirectorySeparatorChar +
            "Rentit21" + Path.DirectorySeparatorChar +
            "Documents" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "Test");
            byte[] bytes = File.ReadAllBytes("D:"+Path.DirectorySeparatorChar+"Games"+Path.DirectorySeparatorChar+"billede.bmp");
            MemoryStream ms = new MemoryStream(bytes);
            fs.Write("BILLEDER.bmp",ms);
        }
    }
}
