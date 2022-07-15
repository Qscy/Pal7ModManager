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
        public XmlDocument CreateModXml(XmlDocument xmlDocument, string FileName)       //创建Xml
        {
            XmlNode node = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "");        //头部声明
            xmlDocument.AppendChild(node);
            XmlNode RootElement = xmlDocument.CreateElement("Root");        //创建根节点
            xmlDocument.AppendChild(RootElement);
            return xmlDocument;
        }
        public XmlDocument AddModXml(XmlDocument xmlDocument, string Name,string Time,string Selection)         //写入Xml
        {
            XmlNode rootNode = xmlDocument.SelectSingleNode("Root");
            XmlElement modNode = xmlDocument.CreateElement("Mod");          //创建Mod节点
            modNode.SetAttribute("Name", Name);         //写入属性，名称，时间，选择
            modNode.SetAttribute("Time", Time);
            modNode.SetAttribute("Selection", Selection);
            rootNode.AppendChild(modNode);      //mod节点加到Root下
            return xmlDocument;
        }
    }
}
