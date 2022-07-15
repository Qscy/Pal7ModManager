using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Pal7ModManager
{
    internal class MyFunctions
    {
        private static DirectoryInfo P7MMPath;
        private static string GamePath;
        public DirectoryInfo GetP7MMPath()         //获取程序路径
        { return P7MMPath; }
        public void SetP7MMPath(DirectoryInfo Path)
        {
            P7MMPath = Path;        //设置程序路径
        }
        public string GetGamePath()         //获取游戏路径
        { return GamePath; }
        public void SetGamePath(string Path)        //设置游戏路径
        {
            GamePath = Path;
        }
        public string ModPath(string FileName,string Extention)        //Mod文件夹下的全路径（文件名，后缀）
        {
            string mp = GetGamePath() + "\\Mods\\" + FileName + Extention;
            return mp;
        }
        public string TargetPath(string FileName, string suffix)        //Paks文件夹下的全路径（文件名，后缀）
        {
            string tp = GetGamePath() + "\\Content\\Paks\\" + FileName + suffix;
            return tp;
        }
        public void ImportMod(string PakPath)       //导入Mod函数（参数：需要导入的Mod所在的全路径）
        {
            string FileName = Path.GetFileNameWithoutExtension(PakPath);        //获取不带后缀文件名
//            DirectoryInfo P7MMPath = new DirectoryInfo(Application.StartupPath);        //获取程序运行目录（Pal7/Pal7ModManager）
//            string GamePath = P7MMPath.Parent.FullName;         //游戏目录（Pal7）
            string ModPakPath = ModPath(FileName,".pak");       //复制后的pak文件全路径
            string ModSigPath = ModPath(FileName,".sig");       //复制后的sig文件全路径
            File.Copy(PakPath, ModPakPath, true);
            File.Copy(PakPath, ModSigPath, true);       //复制文件到Mod文件夹
        }
        public string ApplyMod(string ModName,int Platform)          //应用Mod
        {
            if(File.Exists(ModPath(ModName,".pak")))        //如果Mod的pak文件存在
            {
                if((File.Exists(ModPath(ModName,".sig"))&&(Platform == 1)))        //并且Mod的sig文件存在,且平台是方块
                {
                    File.Copy(ModPath(ModName, ".pak"), TargetPath(ModName, ".pak"),  true);
                    File.Copy(ModPath(ModName, ".sig"), TargetPath(ModName, ".sig"),  true);
                    return "应用成功";
                }
                else
                {
                    foreach(string fileName in Directory.GetFiles(GamePath+"\\Content\\Paks"))          //sig文件不存在或平台不是方块是steam
                    {
                        if (fileName.Contains("WindowsNoEditor.sig"))                           //找一个Paks里的sig改成Mod同名复制
                        {
                            File.Copy(fileName, ModPath(ModName,".sig"), true);
                        }
                    }
                    return "应用成功";
                }
            }
            else        //以上方法都不成功
            {
                return ModName + "不存在";
            }
        }
    }
}
