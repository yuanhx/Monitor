using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using System.Threading;
using ICSharpCode.SharpZipLib.Core;

namespace SDP.Util
{
    public class BCUtil
    {
        public static string TestZip(string data)
        {
            FileStream fs = new FileStream("D:\\test.zip", FileMode.Create);
            GZipOutputStream zos = new GZipOutputStream(fs);

            byte[] buf = StrUtil.ToByteArray(data);

            zos.Write(buf, 0, buf.Length);
            zos.Flush();
            fs.Flush();

            //FileStream fs2 = new FileStream("D:\\test2.zip", FileMode.Create);
            //StreamUtils.Copy(fs,fs2,new byte[1024]);
            //fs2.Flush();
            //fs2.Close();            
            
            MemoryStream s1 = new MemoryStream();         

            //StreamUtils.Copy(fs, s1, new byte[1024]);
            fs.CopyTo(s1);
            s1.Seek(0, SeekOrigin.Begin);

            zos.Close();
            fs.Close();

            //fs = new FileStream("D:\\test.zip", FileMode.Open);

            //MemoryStream s1 = new MemoryStream();
            //StreamUtils.Copy(fs,s1,new byte[1024]);
            //s1.Seek(0, SeekOrigin.Begin);
            GZipInputStream zis = new GZipInputStream(s1);

            MemoryStream rs = new MemoryStream();

            buf = new byte[1024];
            int size = 0;
            while ((size = zis.Read(buf, 0, buf.Length)) > 0)
            {
                rs.Write(buf, 0, size);
            }

            zis.Close();
            fs.Close();

            return StrUtil.FromByteArray(rs.ToArray());
        }

        public static string Compress(string data)
        {
            MemoryStream ms = new MemoryStream();
            GZipOutputStream zs = new GZipOutputStream(ms);

            byte[] buf = StrUtil.ToByteArray(data);

            zs.Write(buf, 0, buf.Length);
            zs.Flush();

            return Encode(ms.ToArray());
        }

        public static string Uncompress(string data)
        {
            MemoryStream ss = new MemoryStream(Decode(data));
            GZipInputStream zs = new GZipInputStream(ss);            

            MemoryStream ms = new MemoryStream();

            byte[] buf = new byte[1024];
            int size = 0;
            while ((size = zs.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, size);
            }
            return StrUtil.FromByteArray(ms.ToArray());
        }

        public static string Encode(string data)
        {
            return data != null ? Convert.ToBase64String(StrUtil.ToByteArray(data)) : "";
        }

        public static string Encode(byte[] data)
        {
            return data != null ? Convert.ToBase64String(data) : "";
        }

        public static string Encode(byte[] data, int offset, int length)
        {
            return data != null ? Convert.ToBase64String(data, offset, length) : "";
        }

        public static byte[] Decode(string data)
        {
            return Convert.FromBase64String(data);
        }

        public static string GetMD5(string data)
        {
            return data;
        }
    }
}
