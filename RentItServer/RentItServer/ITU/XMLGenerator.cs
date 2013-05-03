using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using RentItServer.Utilities;
using System.IO;

namespace RentItServer.ITU
{
    public class XMLGenerator
    {
        //The name of the file that the generated xml document will be based on.
        private static string _configFileName = "config.xml";
        private static XmlDocument _defaultXmlBase;

        /// <summary>
        /// Generates an string representing an xml document, replacing the url and filename element.
        /// </summary>
        /// <param name="cId">Channel id</param>
        /// <param name="relativeFilePath">The absolute filePath of the file to be played</param>
        /// <returns>An xml config file as string</returns>
        public static string GenerateConfig(int cId, string filePath)
        {
            string Url = Controller._defaultUrl + Convert.ToString(cId) + Controller._defaultStreamExtension;
            XmlDocument doc = LoadDefaultXmlBase();

            XmlNodeList urlNodeList = doc.GetElementsByTagName("url");
            XmlNode urlNode = urlNodeList[0];
            urlNode.InnerText = Url;

            XmlNodeList fileNameNodeList = doc.GetElementsByTagName("filename");
            XmlNode fileNameNode = fileNameNodeList[0];
            fileNameNode.InnerText = filePath;

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            doc.WriteTo(xmlTextWriter);
            return stringWriter.ToString();
        }

        private static XmlDocument LoadDefaultXmlBase()
        {
            if (_defaultXmlBase == null)
            {
                XmlDocument doc = new XmlDocument();
                Stream s = File.OpenRead(FilePath.ITUChannelConfigPath.ToString() + _configFileName);
                doc.Load(s);
                _defaultXmlBase = doc;
            }
            return _defaultXmlBase;
        }
    }
}