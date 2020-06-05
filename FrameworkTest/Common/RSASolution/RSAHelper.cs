using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.RSASolution
{
    class RSAHelper
    {
        //public static string Encrypt(RSAKeyPair key, string s)
        //{
        //    var cs = s.ToCharArray();
        //    while (cs.Length % key.chunkSizebiFromHex != 0)
        //    {

        //    }




        //    return "";
        //}

        //public static BigInt biFromHex(string s)
        //{
        //    BigInt result = new BigInt();
        //    for (int i = s.Length, j = 0; i > 0; i -= 4, ++j)
        //    {
        //        result.di
        //    }
        //}


    }

    public class BigInt
    {
        public int[] digits = new int[130];

        public BigInt()
        {
            for (int i = 0; i < digits.Length; i++)
            {
                digits[i] = 0;
            }
        }        
    }

    public class RSAKeyPair
    {
        public RSAKeyPair(string encryptionExponent, string decryptionExponent, string modulus)
        {
            radix = 16;

        }

        public BigInt e { set; get; }
        public BigInt d { set; get; }
        public BigInt m { set; get; }
        public int chunkSize { set; get; }
        public int radix { set; get; }
        public int barrett { set; get; }

        //       var RSAKeyPair = function(encryptionExponent, decryptionExponent, modulus) {
        //   var $dmath = RSAUtils;
        //this.e = $dmath.biFromHex(encryptionExponent);
        //this.d = $dmath.biFromHex(decryptionExponent);
        //this.m = $dmath.biFromHex(modulus);
        //// We can do two bytes per digit, so
        //// chunkSize = 2 * (number of digits in modulus - 1).
        //// Since biHighIndex returns the high index, not the number of digits, 1 has
        //// already been subtracted.
        //this.chunkSize = 2 * $dmath.biHighIndex(this.m);
        //this.radix = 16;
        //this.barrett = new $w.BarrettMu(this.m);
        //       };
        //   }
        //}
    }

    public class PublicKey
    {
        public string exponent { set; get; }
        public int code { set; get; }
        public string modulus { set; get; }
    }
}
