using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Xml;

namespace Pal7ModManager
{
    public partial class P7MM : Form
    {
        MyFunctions mf = new MyFunctions();        //实例化函数
        Xml xml = new Xml();
        public P7MM()
        {
            InitializeComponent();
        }

        private void ImportMod_Click(object sender, EventArgs e)        //当导入Mod菜单被单击
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();       //打开选择框选择pak
            openFileDialog.Filter = "Mod File (*.pak)|*.pak";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mf.ImportMod(Path.GetFullPath(openFileDialog.FileName));        //调用MyFunctions类中的导入Mod函数
            }
        }

        private void P7MM_Load(object sender, EventArgs e)      //窗体加载时执行
        {
            DirectoryInfo P7MMPath = new DirectoryInfo(Application.StartupPath);        //获取程序路径
            mf.SetP7MMPath(P7MMPath);
            mf.SetGamePath(mf.GetP7MMPath().Parent.FullName);       //获取游戏路径
            DirectoryInfo PaksPath = new DirectoryInfo(mf.GetGamePath() + "\\Content\\Paks");       //Paks文件夹路径
            //List<List<string>> Mods = new List<List<string>>();
            List<string> ModName = new List<string>();
            List<DateTime> ModLastTime = new List<DateTime>();
            DirectoryInfo ModsPath = new DirectoryInfo(mf.GetGamePath() + "\\Mods");        //Mods文件夹路径
            foreach (FileInfo ModsFile in ModsPath.GetFiles())          //遍历Mods文件夹
            {
                if (ModsFile.Extension.ToLower() == ".pak")         //后缀为pak时保留
                {
                    string[] Name = ModsFile.Name.Split('.');       //去掉后缀
                    ModName.Add(Name[0]);
                    ModLastTime.Add(ModsFile.LastWriteTime);
                }
            }
            for (int i = 0; i < ModName.Count; i++)
            {
                this.ModList.Items.Add("\t"+ModName[i] + "\t\t\t\t\t\t\t\t" + ModLastTime[i].ToString());       //在选择框上显示Mod名称和修改日期
            }
            List<string> modSelection = new List<string>();
            foreach(object item in ModList.Items)
            {
                if (ModList.GetSelected(ModList.Items.IndexOf(item)))
                {
                    modSelection.Add("1");
                }
                else
                {
                    modSelection.Add("0");
                }
            }
            xml.xmlFileName = Application.StartupPath+ "\\Mods.xml";
            XmlDocument xmlDocument = new XmlDocument();
            if(File.Exists(xml.xmlFileName))
            {
                xmlDocument.Load(xml.xmlFileName);
                xmlDocument.SelectSingleNode("Root").RemoveAll();
                for (int i = 0; i < ModName.Count; i++)
                {
                    xmlDocument = xml.AddModXml(xmlDocument, ModName[i], ModLastTime[i].ToString(), modSelection[i]);
                }
                xmlDocument.Save(xml.xmlFileName);
            }
            else
            {
                xmlDocument = xml.CreateModXml(xmlDocument,xml.xmlFileName);
                for (int i = 0; i < ModName.Count; i++)
                {
                    xmlDocument = xml.AddModXml(xmlDocument, ModName[i], ModLastTime[i].ToString(), modSelection[i]);
                }
                xmlDocument.Save(xml.xmlFileName);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
