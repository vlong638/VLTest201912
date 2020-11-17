using System.Security.Cryptography;
using System.Text;

namespace Autobots.Infrastracture.Common.ValuesSolution
{
    public static class MD5Ex
    {
        /// <summary>
        /// 通过字符串获取MD5值，返回32位字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] MD5Bytes = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < MD5Bytes.Length; i++)
            {
                sb.Append(MD5Bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
