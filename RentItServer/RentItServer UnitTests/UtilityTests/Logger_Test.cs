using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentItServer.ITU;
using RentItServer.Utilities;

namespace RentItServer_UnitTests.UtilityTests
{
    [TestClass]
    public class Logger_Test
    {
        [TestMethod]
        public void Logger_Parameter()
        {
            string absolutePath = FilePath.ITULogPath.GetPath() + "LogFile.txt";
            Logger logger = new Logger(absolutePath);
            FileSystemDao fs = FileSystemDao.GetInstance();
            logger.AddEntry("Entry one");
            logger.AddEntry("Entry two");
            Assert.IsTrue(fs.Exists(absolutePath));
        }
    }
}
