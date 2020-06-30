using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePACKER
{
    public static class Util
    {

        public static string ReadNullString(BinaryReader br)
        {
            string str = ""; char ch;
            while ((ch = br.ReadChar()) != 0) str = str + ch;
            return str;
        }
        
        public static long NextPaddingOffset(uint position, uint paddingSize = 2048)
        {
            if (!(position % paddingSize == 0)){ do{ position++; } while(position % paddingSize != 0); }
            return position;
        }

        public static byte[] GetFileData(BinaryReader br, uint offset, int size)
        {
            unsafe
            {
                br.BaseStream.Position = offset;
                byte[] bytes = new byte[size];
                br.Read(bytes, 0, size);
                return bytes;
            }
        }

        public static void MakeBackup(string file)
        {
            if (File.Exists(file))
            {
                File.Move(file, file + ".bak");
            }
        }
    }
}
