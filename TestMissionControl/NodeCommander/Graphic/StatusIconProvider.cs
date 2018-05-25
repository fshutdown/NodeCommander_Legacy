using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.NodeCommander.Graphic
{
    public static class StatusIconProvider
    {
        private static Bitmap redCircle;
        public static Bitmap RedCircle
        {
            get
            {
                if (redCircle == null) redCircle = CreateStatusBitmaps(Color.Crimson);
                return redCircle;
            }
        }

        private static Bitmap greenCircle;
        public static Bitmap GreenCircle
        {
            get
            {
                if (greenCircle == null) greenCircle = CreateStatusBitmaps(Color.Green);
                return greenCircle;
            }
        }
        private static Bitmap grayCircle;
        public static Bitmap GrayCircle
        {
            get
            {
                if (grayCircle == null) grayCircle = CreateStatusBitmaps(Color.Gray);
                return grayCircle;
            }
        }

        private static Bitmap orangeCircle;
        public static Bitmap OrangeCircle
        {
            get
            {
                if (orangeCircle == null) orangeCircle = CreateStatusBitmaps(Color.Orange);
                return orangeCircle;
            }
        }

        public static Bitmap CreateStatusBitmaps(Color color)
        {
            Bitmap circleBitmap = new Bitmap(16, 16);
            Graphics graphics = Graphics.FromImage(circleBitmap);
            Brush backgroundBrush = new SolidBrush(color);
            Brush foregroundBrush = new SolidBrush(Color.White);
            graphics.FillEllipse(backgroundBrush, 1, 1, 14, 14);
            graphics.FillEllipse(foregroundBrush, 5, 5, 7, 7);

            return circleBitmap;
        }

    }
}
