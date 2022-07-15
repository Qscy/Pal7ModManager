using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


//网上很多这样的代码,懒就复制了一个，源码网址：https://blog.csdn.net/weixin_45499836/article/details/118353614  作者：Pass_Time_

namespace Pal7ModManager
{
    class Ini
    {
        private string _iniFileName = null;
        public string iniFileName
        { get { return _iniFileName; } set { _iniFileName = value; } }
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


        /// 写入INI的方法
        public void IniWrite(string section, string key, string value, string path)
        {
            // section=配置节点名称，key=键名，value=返回键值，path=路径
            WritePrivateProfileString(section, key, value, path);
        }

        //读取INI的方法
        public string IniRead(string section, string key, string path)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);

            // section=配置节点名称，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();

        }

        //删除一个INI文件
        public void IniDelete(string FilePath)
        {
            File.Delete(FilePath);
        }

    }
}
