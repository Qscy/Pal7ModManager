using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pal7ModManager
{
    internal class Xml
    {
        private string _xmlFileName = null;
        public string xmlFileName
        { get
            {
                return _xmlFileName;
            }
            set
            {
                _xmlFileName = value;
            }
        }
        public XmlDocument CreateModXml(XmlDocument xmlDocument, string FileName)
        {
            XmlNode node = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDocument.AppendChild(node);
            XmlNode RootElement = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(RootElement);
            return xmlDocument;
        }
        public XmlDocument AddModXml(XmlDocument xmlDocument, string Name,string Time,string Selection)
        {
            XmlNode rootNode = xmlDocument.SelectSingleNode("Root");
            XmlElement modNode = xmlDocument.CreateElement("Mod");
            modNode.SetAttribute("Name", Name);
            modNode.SetAttribute("Time", Time);
            modNode.SetAttribute("Selection", Selection);
            rootNode.AppendChild(modNode);
            return xmlDocument;
        }
    }
}
