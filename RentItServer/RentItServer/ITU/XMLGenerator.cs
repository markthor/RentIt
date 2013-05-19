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
        /// <summary>
        /// Generates the xml-code for the config file of a channel
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        /// <param name="filePath">The absolute path to the channels corresponding .m3u file</param>
        /// <returns></returns>
        public static string GenerateConfigXML(int cId, string filePath)
        {
            //Set all values
            string startElement = "ezstream";
            string url = Controller._defaultUrl + Convert.ToString(cId) + Controller._defaultStreamExtension;
            string sourcepassword = "hackme";
            string format = "MP3";
            string filename = filePath;
            string stream_once = "0";
            string svrinfoname = "Channel id: " + cId;
            string svrinfourl = "rentit.itu.dk/blobfishradio";
            string svrinfogenre = "";
            string svrinfodescription = "";
            string svrinfobitrate = "128";
            string svrinfochannels = "2";
            string svrinfosamplerate = "44100";
            string svrinfopublic = "0";

            //Set xml settings
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "    ";
            xmlSettings.NewLineChars = "\r\n";
            xmlSettings.NewLineHandling = NewLineHandling.Replace;

            //A stringbuilder to contain the xml output
            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb, xmlSettings);
            //Adds xml versioning
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
            //Make sure everything is flushed to the stringbuilder
            xmlWriter.Flush();
            
            //Return the content of the string builder
            return sb.ToString();
        }
    }
}