using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace KDMSCommon
{
    public static class EncryptHelper
    {
        #region AES

        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string _Key
        {
            get { return @"#KDMSP@ssword!qazXSW@(~A)1&3m_s%"; }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string _IV
        {
            get { return @"L%n67}G\Mk@k%:~Y"; }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(_Key);
            byte[] bIV = Encoding.UTF8.GetBytes(_IV);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            aes.Mode = CipherMode.ECB;
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr, bool returnNull)
        {
            string encrypt = AESEncrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(_Key);
            byte[] bIV = Encoding.UTF8.GetBytes(_IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            aes.Mode = CipherMode.ECB;
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr, bool returnNull)
        {
            string decrypt = AESDecrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        }

        /// <summary>
        /// 随机生成密钥
        /// </summary>
        /// <returns></returns>
        public static string GetIv(int n)
        {
            char[] arrChar = new char[]{
           'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
           '0','1','2','3','4','5','6','7','8','9',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
          };

            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }

            return num.ToString();
        }

        #endregion

        #region DES

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(_Key);
            byte[] bIV = Encoding.UTF8.GetBytes(_IV);

            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);
            string encrypt = string.Empty;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            des.Clear();

            return encrypt;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            des.Clear();

            return decrypt;
        }

        #endregion

        #region MD5加密方法
        /// <summary>
        /// MD5加密方法
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <returns></returns>
        public static string MD5Hash(string originalString)
        {
            //将要加密的字符串转化为byte类型
            byte[] data = System.Text.Encoding.GetEncoding("utf-8").GetBytes(originalString);
            //创建一个Md5对象
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //加密Byte[]数组
            byte[] result = md5.ComputeHash(data);
            //将加密后的数组转化为字段
            string sResult = System.Text.Encoding.GetEncoding("utf-8").GetString(result);
            return sResult;
        }
        #endregion     

        private static string encryptKey = "kdms!";

        /// <summary>
        /// 调用加密算法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateEncryptDes(string data)
        {
            byte[] key = System.Text.Encoding.Unicode.GetBytes(encryptKey);
            byte[] iv = System.Text.Encoding.Unicode.GetBytes(encryptKey);
            return EncryptDes(data,key,iv);
        }

        /// <summary>
        /// 调用解密算法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateDencryptDes(string data)
        {
            byte[] key = System.Text.Encoding.Unicode.GetBytes(encryptKey);
            byte[] iv = System.Text.Encoding.Unicode.GetBytes(encryptKey);
            return DencryptDes(data, key, iv);
        }


        #region 对称加密
        /// <summary>  
        /// DES加密算法必须使用Base64的Byte对象  
        /// </summary>  
        /// <param name="data">待加密的字符数据</param>  
        /// <param name="key">密匙，长度必须为64位（byte[8]）)</param>  
        /// <param name="iv">iv向量，长度必须为64位（byte[8]）</param>  
        /// <returns>加密后的字符</returns>  
        static string EncryptDes(string data, byte[] key, byte[] iv)
        {
            DES des = DES.Create();
            //这行代码很重要,需要根据不同的字符串选择不同的转换格式  
            byte[] tmp = Encoding.Unicode.GetBytes(data);
            Byte[] encryptoData;

            ICryptoTransform encryptor = des.CreateEncryptor(key, iv);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var cs = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cs))
                    {
                        writer.Write(data);
                        writer.Flush();
                    }
                }
                encryptoData = memoryStream.ToArray();
            }
            des.Clear();

            return Convert.ToBase64String(encryptoData);

        }
        #endregion

        #region 对称解密
        /// <summary>  
        /// DES解密算法  
        /// </summary>  
        /// <param name="data">待加密的字符数据</param>  
        /// <param name="key">密匙，长度必须为64位（byte[8]）)</param>  
        /// <param name="iv">iv向量，长度必须为64位（byte[8]）</param>  
        /// <returns>加密后的字符</returns>  
        static string DencryptDes(string data, Byte[] key, Byte[] iv)
        {
            string resultData = string.Empty;
            //这行代码很重要  
            byte[] tmpData = Convert.FromBase64String(data);//转换的格式挺重要  
            DES des = DES.Create();

            ICryptoTransform decryptor = des.CreateDecryptor(key, iv);
            using (var memoryStream = new MemoryStream(tmpData))
            {
                using (var cs = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {

                    StreamReader reader = new StreamReader(cs);
                    resultData = reader.ReadLine();
                }

            }
            return resultData;
        }
        #endregion


        #region 新对称加密解密方法

        private const string Key = "kdmscom";
        private const string IV = "kdms9381c";

        private static readonly byte[] __KEY = Encoding.ASCII.GetBytes(Key);
        private static readonly byte[] __IV = Encoding.ASCII.GetBytes(IV);

        //static EncryptHelper()
        //{
        //    __KEY = Encoding.ASCII.GetBytes(Key);
        //    __IV = Encoding.ASCII.GetBytes(IV);
        //}

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncryptDES(string s)
        {
            var plainData  = Encoding.Unicode.GetBytes(s ?? String.Empty);
            var cipherData = CryptoByDES(plainData, __KEY, __IV, true);

            return Convert.ToBase64String(cipherData);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecryptDES(string s)
        {
            try
            {
                var cipherData = Convert.FromBase64String(s);
                var plainData = CryptoByDES(cipherData, __KEY, __IV, false);

                return Encoding.Unicode.GetString(plainData);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static byte[] CryptoByDES(byte[] data, byte[] key, byte[] iv, bool toCipher = true)
        {
            if ((key == null) || (key.Length <= 0)) throw new ArgumentNullException("key");
            if ((iv == null) || (iv.Length <= 0)) throw new ArgumentNullException("iv");

            using (var des = new DESCryptoServiceProvider())
            using (var ms  = new MemoryStream())
            using (var cs = new CryptoStream(ms, toCipher ? des.CreateEncryptor(key, iv) : des.CreateDecryptor(key, iv), CryptoStreamMode.Write))
            {
                if (!((data == null) || (data.Length <= 0)))
                    cs.Write(data, 0, data.Length);

                cs.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        #endregion
    }
}
