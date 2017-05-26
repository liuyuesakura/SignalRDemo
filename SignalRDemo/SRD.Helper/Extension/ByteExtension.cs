using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ByteExtension
    {
        public static string GetStringFromBytes(this byte[] bs)
        {
            return System.Text.Encoding.Default.GetString(bs);
        }
    }
}
