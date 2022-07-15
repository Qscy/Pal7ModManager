﻿using System;
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
        Ini ini = new Ini();
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
            List<string> modSelection = new List<string>();         //Mod是否选择的列表
            foreach(object item in ModList.Items)
            {
                if (ModList.GetSelected(ModList.Items.IndexOf(item)))       //判断选项是否选中，选中扣1；没选中扣0
                {
                    modSelection.Add("1");
                }
                else
                {
                    modSelection.Add("0");
                }
            }
            xml.xmlFileName = Application.StartupPath+ "\\Mods.xml";        //Mod信息写入XML
            XmlDocument xmlDocument = new XmlDocument();
            if(File.Exists(xml.xmlFileName))        //判断文件是否存在，存在就读、不存在就创建
            {
                xmlDocument.Load(xml.xmlFileName);
                xmlDocument.SelectSingleNode("Root").RemoveAll();           //先跳转到Root节点
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
            ini.iniFileName = Application.StartupPath + "\\P7MM.ini";           //ini信息读取和写入
            if(File.Exists(ini.iniFileName))            //ini文件存在就读不存在就创
            {
                string index = null;
                index = ini.IniRead("MAIN", "Platform", ini.iniFileName);
                try
                {
                    this.PlatFormSelection.SelectedIndex = Convert.ToInt32(index);          //读0或1，其他数全部当-1
                }
                catch
                {
                    this.PlatFormSelection.SelectedIndex = -1;
                }
            }
            else
            {
                ini.IniWrite("MAIN", "Platform", "-1", ini.iniFileName);            //创建默认先为-1（不选择）
                ini.IniWrite("PATH", "GamePath", mf.GetGamePath(), ini.iniFileName);
                ini.IniWrite("PATH", "P7MMPath", mf.GetP7MMPath().ToString(), ini.iniFileName);
                ini.IniWrite("PATH", "ModsPath", ModsPath.ToString(), ini.iniFileName);
                ini.IniWrite("PATH", "PaksPath", PaksPath.ToString(), ini.iniFileName);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            switch (PlatFormSelection.SelectedIndex)            //ini信息写入
            {
                case 0:
                    ini.IniWrite("MAIN", "Platform", "0", ini.iniFileName);     //0对应0；steam
                    break;
                case 1:
                    ini.IniWrite("MAIN", "Platform", "1", ini.iniFileName);     //1对应1；对应方块
                    break;
                default:
                    ini.IniWrite("MAIN", "Platform", "-1", ini.iniFileName);        //其他默认-1；未选择
                    break;
            }
            List<string> ModsName = new List<string>();
            string msgBox = null;
            for(int i = 0; i < ModList.SelectedIndices.Count; i++)
            {
                ModsName.Add(ModList.GetItemText(ModList.Items[ModList.SelectedIndices[i]]));
            }
            for (int i = 0; i < ModsName.Count; i++)
            {
                msgBox += mf.ApplyMod(ModsName[i]);
            }
            MessageBox.Show(msgBox);
        }
    }
}