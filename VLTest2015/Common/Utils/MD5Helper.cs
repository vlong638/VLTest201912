using System.Security.Cryptography;
using System.Text;

namespace VLTest2015.Utils
{
    public class MD5Helper
    {
        public static string GetHashValue(string input)
        {
            using (MD5 mi = MD5.Create())
            {
                //开始加密
                byte[] buffer = Encoding.Default.GetBytes(input);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}