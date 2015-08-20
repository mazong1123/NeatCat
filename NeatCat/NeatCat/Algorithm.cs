//-----------------------------------------------------------------------
// <copyright file="Algorithm.cs" company="Jim Ma">
//     Copyright (c) Jim Ma. All rights reserved.
// </copyright>
// <author>Jim Ma</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NeatCat
{
    /// <summary>
    /// A collection of useful algorithm methods.
    /// </summary>
    public static class Algorithm
    {
        private static Random rand = new Random();

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static string Sha1Hash(string sourceString)
        {
            return string.Join(string.Empty, SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.ASCII.GetBytes(sourceString)).Select(x => x.ToString("x2")));
        }

        /// <summary>
        /// Get upper case md5 result.
        /// </summary>
        /// <param name="encypStr">The string to encrypt.</param>
        /// <param name="charset">The charset for encoding.</param>
        /// <returns>A md5 encrypted string.</returns>
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            // Create md5 object
            byte[] inputBye;
            byte[] outputBye;

            // Convert to byte array.
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", string.Empty).ToUpper();
            return retStr;
        }

        public static string BuildRandomStr(int length)
        {
            int num = rand.Next();

            string str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }

            return str;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Image FixeSize(Image image, int width, int height, bool needToFill)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double scale = 0;
            double scaleW = 0;
            double scaleH = 0;

            scaleW = (double)width / (double)sourceWidth;
            scaleH = (double)height / (double)sourceHeight;
            if (!needToFill)
            {
                scale = Math.Min(scaleH, scaleW);
            }
            else
            {
                scale = Math.Max(scaleH, scaleW);
                destY = (height - (sourceHeight * scale)) / 2;
                destX = (width - (sourceWidth * scale)) / 2;
            }

            if (scale > 1)
            {
                scale = 1;
            }

            int destWidth = (int)Math.Round(sourceWidth * scale);
            int destHeight = (int)Math.Round(sourceHeight * scale);

            System.Drawing.Bitmap bitmapPhoto = null;
            try
            {
                bitmapPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch
            {
                // Failed to resize the image. Keep the original size.
                bitmapPhoto = new System.Drawing.Bitmap(image);
                /*throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);*/
            }

            using (System.Drawing.Graphics graphicPhoto = System.Drawing.Graphics.FromImage(bitmapPhoto))
            {
                graphicPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicPhoto.CompositingQuality = CompositingQuality.HighQuality;
                graphicPhoto.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);

                graphicPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bitmapPhoto;
            }
        }
    }
}
