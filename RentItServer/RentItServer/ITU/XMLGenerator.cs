using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using RentItServer.Utilities;
using System.IO;
using System.Text;

namespace RentItServer.ITU
{
    public class XMLGenerator
    {
        //The name of the file that the generated xml document will be based on.
        private static string _configFileName = "config.xml";
        private static XmlDocument _defaultXmlBase;

        /*/// <summary>
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
            fileNameNode.InnerText = filePath.ToString();

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            doc.WriteTo(xmlTextWriter);
            return stringWriter.ToString();
        }

        private static XmlDocument LoadDefaultXmlBase()
        {
            if (_defaultXmlBase == null)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    Stream s = File.OpenRead(FilePath.ITUChannelConfigPath.GetPath() + _configFileName);
                    doc.Load(s);
                    _defaultXmlBase = doc;
                }
                catch (FileNotFoundException e)
                {
                    throw new FileNotFoundException("The default xml config file named " + _configFileName + " does not exist in " + FilePath.ITUChannelConfigPath.GetPath());
                }
            }
            return _defaultXmlBase;
        }*/


        public static string GenerateConfig(int cId, string filePath)
        {
            string startElement = "ezstream";
            string url = Controller._defaultUrl + Convert.ToString(cId) + Controller._defaultStreamExtension;
            string sourcepassword = "hackme";
            string format = "MP3";
            string filename = filePath;
            string stream_once = "0";
            string svrinfoname = "My Stream";
            string svrinfourl = "http://www.oddsock.org";
            string svrinfogenre = "RockNRoll";
            string svrinfodescription = "This is a stream description";
            string svrinfobitrate = "128";
            string svrinfochannels = "2";
            string svrinfosamplerate = "44100";
            string svrinfopublic = "0";



            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb);
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement(startElement);

            xmlWriter.WriteElementString("url", url);
            xmlWriter.WriteElementString("sourcepassword", sourcepassword);
            xmlWriter.WriteElementString("format", format);
            xmlWriter.WriteElementString("filename", filename);
            xmlWriter.WriteElementString("stream_once", stream_once);
            xmlWriter.WriteElementString("svrinfoname", svrinfoname);
            xmlWriter.WriteElementString("svrinfourl", svrinfourl);
            xmlWriter.WriteElementString("svrinfogenre", svrinfogenre);
            xmlWriter.WriteElementString("svrinfodescription", svrinfodescription);
            xmlWriter.WriteElementString("svrinfobitrate", svrinfobitrate);
            xmlWriter.WriteElementString("svrinfochannels", svrinfochannels);
            xmlWriter.WriteElementString("svrinfosamplerate", svrinfosamplerate);
            xmlWriter.WriteElementString("svrinfopublic", svrinfopublic);

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            return sb.ToString();
        }
    }
}