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
            FontFamily[] ff = FontFamily.Families;
            int fontCount = fonts.Length;
            var thef = new Font("STXINWEI", fontSize, FontStyle.Regular);
            Bitmap bmp = new Bitmap(width, height);

            Graphics graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.White); //清除画面，填充背景
            Random random = new Random();
            try
            {
                //随机噪点
                //for (int i = 0; i < 100; i++)
                //{
                //    int x = random.Next(0, bmp.Width);
                //    int y = random.Next(0, bmp.Height);
                //    bmp.SetPixel(x, y, colors[random.Next(0, colorCount)]);
                //}
                char[] chars = code.ToArray();
                //文字居中
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                int spaceWith = (width - fontSize * chars.Length - 10) / chars.Length;

                for (int i = 0; i < chars.Length; i++)
                {

                    int colorIndex = random.Next(0, colorCount);
                    int fontIndex = random.Next(0, fontCount);
                    FontStyle fs = fontStyle[random.Next(0, fontStyle.Length)];
                    Font f = thef;//new Font(fonts[fontIndex], fontSize, fs); //字体样式
                    Brush b = new SolidBrush(colors[colorIndex]);

                    Point dot = new Point();
                    //int py = random.Next(-2, 2);
                    dot.Y = 4;// (height - f.Height) / 2 - 2;// +py;//angle > 0 ? height - 1 : 0
                    dot.X = i * fontSize + (i + 1) * spaceWith;//i * fontSize + (i + 1) * spaceWith;
                    //singleGrap.TranslateTransform(width,height);

                    //绘制到主图片上

                    float angle = random.Next(-5, 5);
                    if (angle < 0)
                        bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    //graphics.DrawImage(singleChar, new Point(i * fontSize + (i + 1) * spaceWith, 0));
                    graphics.DrawString(chars[i].ToString(), f, b, dot);
                }

                bmp.Save(stram, ImageFormat.Png);
                byte[] result = stram.ToArray();
                bool[,] lattice = GetLatticeFromBitmap(bmp);
                stram.Close();
                bmp.Dispose();
                return result;

            }
            finally
            {
                graphics.Dispose();
            }

        }
        /// <summary>
        /// 获取点阵
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public bool[,] GetLatticeFromBitmap(Bitmap bmp)
        {
            bool[,] result = new bool[bmp.Width, bmp.Height];
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {

                    Color color = bmp.GetPixel(i, j);
                    result[i, j] = color.Name != "ffffffff";
                }
            }
            return result;
        }
        /// <summary>
        /// 判断是否有覆盖到点阵上
        /// </summary>
        /// <param name="lattice">点阵</param>
        /// <param name="point">点击位置</param>
        /// <param name="coverkind">覆盖类型，采用何种方式判断</param>
        /// <param name="range">圆形覆盖则为半径，正方形覆盖为边长</param>
        /// <returns></returns>
        public bool IsCover(bool[,] lattice,Point point,int coverkind,int range)
        {
            IsCoverCheck check = IsCoverCheck_Base.GetCheck(coverkind);

            return check.Check(lattice,point,range);
        }
        interface IsCoverCheck
        {
            bool Check(bool[,] lattice, Point point, int range);
        }
        public class IsCoverCheck_Base : IsCoverCheck
        {
            public bool Check(bool[,] lattice, Point point, int range)
            {
                return true;
            }
            public static IsCoverCheck_Base GetCheck(int coverkind)
            {
                switch (coverkind)
                {
                    case (int)CoverKind.Circle:
                return new IsCoverCheck_Circle();
                    case (int) CoverKind.Square:
                return new IsCoverCheck_Square();
                    default :
                        return null;
                }
            }

        }
        private class IsCoverCheck_Circle : IsCoverCheck_Base
        {
            bool Check(bool[,] lattice, Point point, int range)
            {
                try
                {
                    int count = 0;
                    for (int i = point.X - range; i <= point.X + range; i++)
                    {
                        for (int j = point.Y - range; j <= point.Y + range; j++)
                        {
                            if (
                                (Math.Pow(Math.Abs(i - point.X), 2) + Math.Pow(Math.Abs(j - point.Y), 2)) <= range)
                            {
                                if (lattice[i, j] == true)
                                    count++;
                            }
                        }
                    }
                    if (count > 5)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    
                    return false;
                }
            }
        }
        private class IsCoverCheck_Square : IsCoverCheck_Base
        {
            bool Check(bool[,] lattice, Point point, int range)
            {
                try
                {
                    int count = 0;
                    for (int i = point.X - range; i <= point.X + range; i++)
                    {
                        for (int j = point.Y - range; j <= point.Y + range; j++)
                        {
                            if (lattice[i, j] == true)
                                count++;
                        }
                    }
                    if (count > 5)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }
        public enum CoverKind
        {

            Circle = 0,
            Square = 1
        }
    }
}
