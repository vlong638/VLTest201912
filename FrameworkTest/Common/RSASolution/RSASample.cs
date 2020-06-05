using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.RSASolution
{
    //public key
    //"exponent":"010001"
    //"modulus":"00af8dfa5a14e97e58cac7238a5d4ca89478cedcfd196ea643735d64c74df659cd259c8bd60ec046c4d3f6dec3965dc0351f117f8a0ae62ad61c3a41d38c6a93215025c658587f4aa7ceaa9ed08c2ced8873254c417a77403aff9a0abb3bc1d2ff42f856e1a4d447ed0a1626e1099f304b6602e69cdca1a376ae6bf0dad13844cf"
    //密码
    //123
    //加密结果
    //2d36cfe9d49ccdb6cd313c75a7f4308036092f701a068f7fa66ab1835cd03baa3cbc80191e3bf502453d0cacec215a51adcfb883aa24ecc09025b6dc68d9cca20c722dc3e766e92fb15103b434a6c5fc640bbf7937f016c63a11ecad72018a30b0800a67f21d57f6014057f49c29595e7c3f9e5d1874e109a8e9c37be46ce59b

    #region 模拟登陆对方RSA算法内容整理,改写为C#成本大,发生变动后成本也大,考虑用页面直接接入对方js
    public static class RSAUtils
    {

        //private static string Encrypt(RSAKeyPair key, string s)
        //{
        //    var a = [];
        //    var sl = s.length;
        //    var i = 0;
        //    while (i < sl)
        //    {
        //        a[i] = s.charCodeAt(i);
        //        i++;
        //    }

        //    while (a.length % key.chunkSizebiFromHex != 0)
        //    {
        //        a[i++] = 0;
        //    }

        //    var al = a.length;
        //    var result = "";
        //    var j, k, block;
        //    for (i = 0; i < al; i += key.chunkSize)
        //    {
        //        block = new BigInt();
        //        j = 0;
        //        for (k = i; k < i + key.chunkSize; ++j)
        //        {
        //            block.digits[j] = a[k++];
        //            block.digits[j] += a[k++] << 8;
        //        }
        //        var crypt = key.barrett.powMod(block, key.e);
        //        var text = key.radix == 16 ? RSAUtils.biToHex(crypt) : RSAUtils.biToString(crypt, key.radix);
        //        result += text + " ";
        //    }
        //    return result.substring(0, result.length - 1); // Remove last space.
        //}

        //biMultiply = function(x, y)
        //{
        //    var result = new BigInt();
        //    var c;
        //    var n = RSAUtils.biHighIndex(x);
        //    var t = RSAUtils.biHighIndex(y);
        //    var u, uv, k;

        //    for (var i = 0; i <= t; ++i)
        //    {
        //        c = 0;
        //        k = i;
        //        for (j = 0; j <= n; ++j, ++k)
        //        {
        //            uv = result.digits[k] + x.digits[j] * y.digits[i] + c;
        //            result.digits[k] = uv & maxDigitVal;
        //            c = uv >>> biRadixBits;
        //            //c = Math.floor(uv / biRadix);
        //        }
        //        result.digits[i + n + 1] = c;
        //    }
        //    // Someone give me a logical xor, please.
        //    result.isNeg = x.isNeg != y.isNeg;
        //    return result;
        //};
        //biDivideByRadixPower = function(x, n)
        //{
        //    var result = new BigInt();
        //    RSAUtils.arrayCopy(x.digits, n, result.digits, 0, result.digits.length - n);
        //    return result;
        //};
        //biModuloByRadixPower = function(x, n)
        //{
        //    var result = new BigInt();
        //    RSAUtils.arrayCopy(x.digits, 0, result.digits, 0, n);
        //    return result;
        //};

        //biMultiply = function(x, y)
        //{
        //    var result = new BigInt();
        //    var c;
        //    var n = RSAUtils.biHighIndex(x);
        //    var t = RSAUtils.biHighIndex(y);
        //    var u, uv, k;

        //    for (var i = 0; i <= t; ++i)
        //    {
        //        c = 0;
        //        k = i;
        //        for (j = 0; j <= n; ++j, ++k)
        //        {
        //            uv = result.digits[k] + x.digits[j] * y.digits[i] + c;
        //            result.digits[k] = uv & maxDigitVal;
        //            c = uv >>> biRadixBits;
        //            //c = Math.floor(uv / biRadix);
        //        }
        //        result.digits[i + n + 1] = c;
        //    }
        //    // Someone give me a logical xor, please.
        //    result.isNeg = x.isNeg != y.isNeg;
        //    return result;
        //};

        //modulo = function BarrettMu_modulo(x)
        //{
        //    var $dmath = RSAUtils;
        //    var q1 = $dmath.biDivideByRadixPower(x, this.k - 1);
        //    var q2 = $dmath.biMultiply(q1, this.mu);
        //    var q3 = $dmath.biDivideByRadixPower(q2, this.k + 1);
        //    var r1 = $dmath.biModuloByRadixPower(x, this.k + 1);
        //    var r2term = $dmath.biMultiply(q3, this.modulus);
        //    var r2 = $dmath.biModuloByRadixPower(r2term, this.k + 1);
        //    var r = $dmath.biSubtract(r1, r2);
        //    if (r.isNeg)
        //    {
        //        r = $dmath.biAdd(r, this.bkplus1);
        //    }
        //    var rgtem = $dmath.biCompare(r, this.modulus) >= 0;
        //    while (rgtem)
        //    {
        //        r = $dmath.biSubtract(r, this.modulus);
        //        rgtem = $dmath.biCompare(r, this.modulus) >= 0;
        //    }
        //    return r;
        //}

        //this.multiplyMod = function BarrettMu_multiplyMod(x, y)
        //{
        //    /*
        //    x = this.modulo(x);
        //    y = this.modulo(y);
        //    */
        //    var xy = RSAUtils.biMultiply(x, y);
        //    return this.modulo(xy);
        //}

        //TODO powMod
        //barrett.powMod = function BarrettMu_powMod(x, y)
        //{
        //    var result = new BigInt();
        //    result.digits[0] = 1;
        //    var a = x;
        //    var k = y;
        //    while (true)
        //    {
        //        if ((k.digits[0] & 1) != 0) result = this.multiplyMod(result, a);
        //        k = RSAUtils.biShiftRight(k, 1);
        //        if (k.digits[0] == 0 && RSAUtils.biHighIndex(k) == 0) break;
        //        a = this.multiplyMod(a, a);
        //    }
        //    return result;
        //}

        //var hexatrigesimalToChar = [
        //    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        //    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        //    'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        //    'u', 'v', 'w', 'x', 'y', 'z'
        //];

        //biCompare = function(x, y)
        //{
        //    if (x.isNeg != y.isNeg)
        //    {
        //        return 1 - 2 * Number(x.isNeg);
        //    }
        //    for (var i = x.digits.length - 1; i >= 0; --i)
        //    {
        //        if (x.digits[i] != y.digits[i])
        //        {
        //            if (x.isNeg)
        //            {
        //                return 1 - 2 * Number(x.digits[i] > y.digits[i]);
        //            }
        //            else
        //            {
        //                return 1 - 2 * Number(x.digits[i] < y.digits[i]);
        //            }
        //        }
        //    }
        //    return 0;
        //};

        //biToString = function(x, radix)
        //{ // 2 <= radix <= 36
        //    var b = new BigInt();
        //    b.digits[0] = radix;
        //    var qr = RSAUtils.biDivideModulo(x, b);
        //    var result = hexatrigesimalToChar[qr[1].digits[0]];
        //    while (RSAUtils.biCompare(qr[0], bigZero) == 1)
        //    {
        //        qr = RSAUtils.biDivideModulo(qr[0], b);
        //        digit = qr[1].digits[0];
        //        result += hexatrigesimalToChar[qr[1].digits[0]];
        //    }
        //    return (x.isNeg ? "-" : "") + RSAUtils.reverseStr(result);
        //};


        //biDivideModulo = function(x, y) {
        //	var nb = RSAUtils.biNumBits(x);
        //	var tb = RSAUtils.biNumBits(y);
        //	var origYIsNeg = y.isNeg;
        //	var q, r;
        //	if (nb < tb) {
        //		// |x| < |y|
        //		if (x.isNeg) {
        //			q = RSAUtils.biCopy(bigOne);
        //			q.isNeg = !y.isNeg;
        //			x.isNeg = false;
        //			y.isNeg = false;
        //			r = biSubtract(y, x);
        //			// Restore signs, 'cause they're references.
        //			x.isNeg = true;
        //			y.isNeg = origYIsNeg;
        //		} else {
        //			q = new BigInt();
        //			r = RSAUtils.biCopy(x);
        //		}
        //		return [q, r];
        //	}

        //	q = new BigInt();
        //	r = x;

        //	// Normalize Y.
        //	var t = Math.ceil(tb / bitsPerDigit) - 1;
        //	var lambda = 0;
        //	while (y.digits[t] < biHalfRadix) {
        //		y = RSAUtils.biShiftLeft(y, 1);
        //		++lambda;
        //		++tb;
        //		t = Math.ceil(tb / bitsPerDigit) - 1;
        //	}
        //	// Shift r over to keep the quotient constant. We'll shift the
        //	// remainder back at the end.
        //	r = RSAUtils.biShiftLeft(r, lambda);
        //	nb += lambda; // Update the bit count for x.
        //	var n = Math.ceil(nb / bitsPerDigit) - 1;

        //	var b = RSAUtils.biMultiplyByRadixPower(y, n - t);
        //	while (RSAUtils.biCompare(r, b) != -1) {
        //		++q.digits[n - t];
        //		r = RSAUtils.biSubtract(r, b);
        //	}
        //	for (var i = n; i > t; --i) {
        //    var ri = (i >= r.digits.length) ? 0 : r.digits[i];
        //    var ri1 = (i - 1 >= r.digits.length) ? 0 : r.digits[i - 1];
        //    var ri2 = (i - 2 >= r.digits.length) ? 0 : r.digits[i - 2];
        //    var yt = (t >= y.digits.length) ? 0 : y.digits[t];
        //    var yt1 = (t - 1 >= y.digits.length) ? 0 : y.digits[t - 1];
        //		if (ri == yt) {
        //			q.digits[i - t - 1] = maxDigitVal;
        //		} else {
        //			q.digits[i - t - 1] = Math.floor((ri * biRadix + ri1) / yt);
        //		}

        //		var c1 = q.digits[i - t - 1] * ((yt * biRadix) + yt1);
        //		var c2 = (ri * biRadixSquared) + ((ri1 * biRadix) + ri2);
        //		while (c1 > c2) {
        //			--q.digits[i - t - 1];
        //			c1 = q.digits[i - t - 1] * ((yt * biRadix) | yt1);
        //			c2 = (ri * biRadix * biRadix) + ((ri1 * biRadix) + ri2);
        //		}

        //		b = RSAUtils.biMultiplyByRadixPower(y, i - t - 1);
        //		r = RSAUtils.biSubtract(r, RSAUtils.biMultiplyDigit(b, q.digits[i - t - 1]));
        //		if (r.isNeg) {
        //			r = RSAUtils.biAdd(r, b);
        //			--q.digits[i - t - 1];
        //		}
        //	}
        //	r = RSAUtils.biShiftRight(r, lambda);
        //	// Fiddle with the signs and stuff to make sure that 0 <= r < y.
        //	q.isNeg = x.isNeg != origYIsNeg;
        //	if (x.isNeg) {
        //		if (origYIsNeg) {
        //			q = RSAUtils.biAdd(q, bigOne);
        //		} else {
        //			q = RSAUtils.biSubtract(q, bigOne);
        //		}
        //		y = RSAUtils.biShiftRight(y, lambda);
        //		r = RSAUtils.biSubtract(y, r);
        //	}
        //	// Check for the unbelievably stupid degenerate case of r == -0.
        //	if (r.digits[0] == 0 && RSAUtils.biHighIndex(r) == 0) r.isNeg = false;

        //	return [q, r];
        //};


        //var BigInt = $w.BigInt = function(flag)
        //{
        //    if (typeof flag == "boolean" && flag == true)
        //    {
        //        this.digits = null;
        //    }
        //    else
        //    {
        //        this.digits = ZERO_ARRAY.slice(0);
        //    }
        //    this.isNeg = false;
        //};

        //var hexToChar = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        //'a', 'b', 'c', 'd', 'e', 'f'];

        //reverseStr = function(s)
        //{
        //    var result = "";
        //    for (var i = s.length - 1; i > -1; --i)
        //    {
        //        result += s.charAt(i);
        //    }
        //    return result;
        //};


        //digitToHex = function(n)
        //{
        //    var mask = 0xf;
        //    var result = "";
        //    for (i = 0; i < 4; ++i)
        //    {
        //        result += hexToChar[n & mask];
        //        n >>>= 4;
        //    }
        //    return RSAUtils.reverseStr(result);
        //};


        //biHighIndex = function(x)
        //{
        //    var result = x.digits.length - 1;
        //    while (result > 0 && x.digits[result] == 0) --result;
        //    return result;
        //};


        //biToHex = function(x)
        //{
        //    var result = "";
        //    var n = RSAUtils.biHighIndex(x);
        //    for (var i = RSAUtils.biHighIndex(x); i > -1; --i)
        //    {
        //        result += RSAUtils.digitToHex(x.digits[i]);
        //    }
        //    return result;
        //};

        //biFromHex = function(s)
        //{
        //    var result = new BigInt();
        //    var sl = s.length;
        //    for (var i = sl, j = 0; i > 0; i -= 4, ++j)
        //    {
        //        result.digits[j] = RSAUtils.hexToDigit(s.substr(Math.max(i - 4, 0), Math.min(i, 4)));
        //    }
        //    return result;
        //};

        //hexToDigit = function(s)
        //{
        //    var result = 0;
        //    var sl = Math.min(s.length, 4);
        //    for (var i = 0; i < sl; ++i)
        //    {
        //        result <<= 4;
        //        result |= RSAUtils.charToHex(s.charCodeAt(i));
        //    }
        //    return result;
        //};

        //charToHex = function(c)
        //{
        //    var ZERO = 48;
        //    var NINE = ZERO + 9;
        //    var littleA = 97;
        //    var littleZ = littleA + 25;
        //    var bigA = 65;
        //    var bigZ = 65 + 25;
        //    var result;

        //    if (c >= ZERO && c <= NINE)
        //    {
        //        result = c - ZERO;
        //    }
        //    else if (c >= bigA && c <= bigZ)
        //    {
        //        result = 10 + c - bigA;
        //    }
        //    else if (c >= littleA && c <= littleZ)
        //    {
        //        result = 10 + c - littleA;
        //    }
        //    else
        //    {
        //        result = 0;
        //    }
        //    return result;
        //};
    }

    //public class RSAKeyPair
    //{
    //    public RSAKeyPair(string exponent, string modulus)
    //    {
    //        radix = 16;
    //    }

    //    public string e { set; get; }
    //    public string d { set; get; }
    //    public string m { set; get; }
    //    public int chunkSize { set; get; }
    //    public int radix { set; get; }
    //    public int barrett { set; get; }

    //    //       var RSAKeyPair = function(encryptionExponent, decryptionExponent, modulus) {
    //    //   var $dmath = RSAUtils;
    //    //this.e = $dmath.biFromHex(encryptionExponent);
    //    //this.d = $dmath.biFromHex(decryptionExponent);
    //    //this.m = $dmath.biFromHex(modulus);
    //    //// We can do two bytes per digit, so
    //    //// chunkSize = 2 * (number of digits in modulus - 1).
    //    //// Since biHighIndex returns the high index, not the number of digits, 1 has
    //    //// already been subtracted.
    //    //this.chunkSize = 2 * $dmath.biHighIndex(this.m);
    //    //this.radix = 16;
    //    //this.barrett = new $w.BarrettMu(this.m);
    //    //       };
    //    //   }
    //    //}
    //}

    //public class PublicKey
    //{
    //    public string exponent { set; get; }
    //    public int code { set; get; }
    //    public string modulus { set; get; }
    //}
    #endregion
}
