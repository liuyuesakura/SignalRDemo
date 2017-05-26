using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtension
    {
        public static byte[] ToBtyes(this string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
    }
}
