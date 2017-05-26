using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SRD.Helper
{
    public sealed class ImageHelper
    {
        public static readonly ImageHelper Instance = new ImageHelper();
        public byte[] CreateVerifyImage(string code, int width, int height, int fontSize)
        {
            MemoryStream stram = new MemoryStream();
            Bitmap greatBmp = new Bitmap(width * 8, height * 8);
            Graphics greatGrap = Graphics.FromImage(greatBmp);
            Color[] colors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.DarkBlue };
            int colorCount = colors.Length;
            string[] fonts = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            FontStyle[] fontStyle = {
                                        FontStyle.Bold,
                                        FontStyle.Italic,
                                        FontStyle.Regular,
                                        FontStyle.Strikeout,
                                        FontStyle.Underline
                                    };
            int fontCount = fonts.Length;
            Bitmap bmp = new Bitmap(width, height);

            Graphics graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.White); //清除画面，填充背景
            Random random = new Random();
            try
            {
                //随机噪点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(0, bmp.Width);
                    int y = random.Next(0, bmp.Height);
                    bmp.SetPixel(x, y, colors[random.Next(0, colorCount)]);
                }
                char[] chars = code.ToArray();
                //文字居中
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                int spaceWith = (width - fontSize * chars.Length - 10) / chars.Length;

                for (int i = 0; i < chars.Length; i++)
                {
                    using (Bitmap singleChar = new Bitmap(spaceWith,height))
                    {
                        singleChar.MakeTransparent(Color.Transparent);

                        int colorIndex = random.Next(0, colorCount);
                        int fontIndex = random.Next(0, fontCount);
                        FontStyle fs = fontStyle[random.Next(0, fontStyle.Length)];
                        Font f = new Font(fonts[fontIndex], fontSize, fs); //字体样式
                        Brush b = new SolidBrush(colors[colorIndex]);

                        float angle = random.Next(-5, 5);
                        using (Graphics singleGrap = Graphics.FromImage(singleChar))
                        {
                            Point dot = new Point();
                            //int py = random.Next(-2, 2);
                            dot.Y = 1;// (height - f.Height) / 2 - 2;// +py;//angle > 0 ? height - 1 : 0
                            dot.X = 0;//i * fontSize + (i + 1) * spaceWith;
                            //singleGrap.TranslateTransform(width,height);
                            singleGrap.RotateTransform(angle);

                            singleGrap.DrawString(chars[i].ToString(), f, b, dot);
                            //singleGrap.TranslateTransform(-width, -height);
                            singleGrap.RotateTransform(angle);

                            //绘制到主图片上
                            if(angle < 0)
                                singleChar.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            graphics.DrawImage(singleChar, new Point(i * fontSize + (i + 1) * spaceWith, 0));

                        }
                    }
                }

               bmp.Save(stram, ImageFormat.Png);
                byte[] result = stram.ToArray();
                stram.Close();
                bmp.Dispose();
                return result;

            }
            finally
            {
                graphics.Dispose();
            }

        }
    }
}
