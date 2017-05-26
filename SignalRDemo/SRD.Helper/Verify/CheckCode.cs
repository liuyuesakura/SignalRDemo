using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRD.Helper.Verify
{
    public sealed class CheckCode
    {
        private Random _random = new Random();

        public static readonly CheckCode Instance = new CheckCode();

        /// <summary>
        /// 随机生成验证码
        /// </summary>
        /// <returns></returns>
        public string CreateCheckCode(int length = 8,int type = 0)
        {
            string res = string.Empty;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] result = new byte[2];
            using (System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                for (int iii = 0; iii < length; iii++)
                {
                    rng.GetBytes(result);
                    while (result[0] > 247 || result[0] < 176)
                    {
                        byte[] newb = new byte[1];
                        rng.GetBytes(newb);
                        int theint = (Convert.ToInt32(result[0]) + Convert.ToInt32(newb[0]));
                        if (theint > 255)
                        {
                            theint = theint / 2;
                        }
                        result[0] = Convert.ToByte(theint);
                        if (result[0] > 247 || result[0] < 176)
                        {
                            result[0] = Convert.ToByte(result[0] / 4);
                        }
                    }
                    while (result[1] > 254 || result[1] < 160)
                    {
                        byte[] newb = new byte[1];
                        rng.GetBytes(newb);
                        int theint = (Convert.ToInt32(result[1]) + Convert.ToInt32(newb[0]));
                        if (theint > 255)
                        {
                            theint = theint / 2;
                        }
                        result[1] = Convert.ToByte(theint);
                        if (result[1] > 254 || result[0] < 160)
                        {
                            result[1] = Convert.ToByte(result[1] / 4);
                        }
                        //result[1] = Convert.ToByte((Convert.ToInt32(result[1]) + Convert.ToInt32(newb[0])) / 16);
                    }
                    string code = Encoding.GetEncoding("gb2312").GetString(result);
                    if (!string.IsNullOrWhiteSpace(code.Trim()) && code.Trim() != "?")
                        res += code;
                    else
                        iii--;
                }
            }
            return res;
        }
        public enum CheckCodeType
        {
            /// <summary>
            /// 汉字
            /// </summary>
            Chn_Character = 0,
            /// <summary>
            /// 数字
            /// </summary>
            Num_Latter = 1,
            /// <summary>
            /// 英文字母
            /// </summary>
            Eng_Latter = 2,
            /// <summary>
            /// 数字英文混合
            /// </summary>
            Num_Eng_Mix = 3,
        }
    }
}
