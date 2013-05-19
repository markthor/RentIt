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

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "    ";
            xmlSettings.NewLineChars = "\r\n";
            xmlSettings.NewLineHandling = NewLineHandling.Replace;

            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb, xmlSettings);
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