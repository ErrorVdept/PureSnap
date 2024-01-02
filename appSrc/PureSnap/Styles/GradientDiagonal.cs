﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

namespace PureSnap.Styles
{
    public class GradientDiagonal
    {
        private static PropertyInfo dpiX_;
        private static PropertyInfo dpiY_;
        private static byte[,] bayerMatrix_ =
        {
            { 1, 9, 3, 11 },
            { 13, 5, 15, 7 },
            { 1, 9, 3, 11 },
            { 16, 8, 14, 6 }
        };
        static GradientDiagonal()
        {
            dpiX_ = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            dpiY_ = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
        }
        /// <summary>
        /// Gradient color at the top
        /// </summary>
        public System.Windows.Media.Color From { get; set; }

        /// <summary>
        /// Gradient color at the bottom
        /// </summary>
        public System.Windows.Media.Color To { get; set; }

        public BitmapSource GenerateBackground(System.Windows.Media.Color from, System.Windows.Media.Color to, int width, int height)
        {
            From = from;
            To = to;
            //If user changes dpi/virtual screen height during applicaiton lifetime,
            //wpf will scale the image up for us.
            //int width = 20;
            //int height = (int)SystemParameters.VirtualScreenHeight;
            int dpix = (int)dpiX_.GetValue(null);
            int dpiy = (int)dpiY_.GetValue(null);


            int stride = 4 * ((width * PixelFormats.Bgr24.BitsPerPixel + 31) / 32);

            //dithering parameters
            double bayerMatrixCoefficient = 1.0 / (bayerMatrix_.Length + 1);
            int bayerMatrixSize = bayerMatrix_.GetLength(0);

            //Create pixel data of image
            byte[] buffer = new byte[height * stride];

            for (int line = 0; line < height; line++)
            {
                double scale = (double)line / height;

                for (int x = 0; x < width * 3; x += 3)
                {
                    double diagonalScale = (scale + (double)x / (width * 3)) / 2;
                    //scaling of color
                    double blue = ((To.B * diagonalScale) + (From.B * (1.0 - diagonalScale)));
                    double green = ((To.G * diagonalScale) + (From.G * (1.0 - diagonalScale)));
                    double red = ((To.R * diagonalScale) + (From.R * (1.0 - diagonalScale)));

                    buffer[x + line * stride] = (byte)(blue + bayerMatrixCoefficient * bayerMatrix_[x % bayerMatrixSize, line % bayerMatrixSize]);
                    buffer[x + line * stride + 1] = (byte)(green + bayerMatrixCoefficient * bayerMatrix_[x % bayerMatrixSize, line % bayerMatrixSize]);
                    buffer[x + line * stride + 2] = (byte)(red + bayerMatrixCoefficient * bayerMatrix_[x % bayerMatrixSize, line % bayerMatrixSize]);


                }
            }

            var image = BitmapSource.Create(width, height, dpix, dpiy, PixelFormats.Bgr24, null, buffer, stride);


            image.Freeze();
            
            return image;
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
    }
}
