using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace GTAVSpeedRadar
{
    public class INIFile
    {
        public static string filePath;

        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section,
        string key,
        string val,
        string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section,
        string key,
        string def,
        StringBuilder retVal,
        int size,
        string filePath);

        public INIFile(string FilePath)
        {
            filePath = FilePath;
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value.ToLower(), filePath);
        }

        public string Read(string section, string key)
        {
            StringBuilder SB = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", SB, 255, filePath);
            return SB.ToString();
        }

        public static string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
    }
}
