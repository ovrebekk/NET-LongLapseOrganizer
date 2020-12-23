using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encoder = System.Drawing.Imaging.Encoder;

namespace LongLapseOrganizer
{
    class ImageProcessing
    {
        private static int THUMB_WIDTH = 600;
        private static int THUMB_HEIGHT = 400;
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Image image, int width, int height)
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

        private static byte[] extractJpegFromRaw(string fileName)
        {
            byte[] retVal = null;
            int offset, length;
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(fileName);
                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                offset = subIfdDirectory.GetInt32(0x0201);
                length = subIfdDirectory.GetInt32(0x0202);
            }
            catch (IOException e)
            {
                return null;
            }
            using (FileStream fs = File.OpenRead(fileName))
            {
                retVal = new byte[length];
                fs.Position = offset;
                fs.Read(retVal, 0, length);
            }
            return retVal;
        }


        /*! \brief Convert RGB to HSV color space
  
          Converts a given set of RGB values `r', `g', `b' into HSV
          coordinates. The input RGB values are in the range [0, 1], and the
          output HSV values are in the ranges h = [0, 360], and s, v = [0,
          1], respectively.
  
          \param fR Red component, used as input, range: [0, 1]
          \param fG Green component, used as input, range: [0, 1]
          \param fB Blue component, used as input, range: [0, 1]
          \param fH Hue component, used as output, range: [0, 360]
          \param fS Hue component, used as output, range: [0, 1]
          \param fV Hue component, used as output, range: [0, 1]
  
        */
        public static void RGBtoHSV(float fR, float fG, float fB, out float fH, out float fS, out float fV)
        {
            float fCMax = (fR > fG) ? (fR > fB ? fR : fB) : (fB > fG ? fB : fG);
            float fCMin = (fR < fG) ? (fR < fB ? fR : fB) : (fB < fG ? fB : fG);
            float fDelta = fCMax - fCMin;

            if (fDelta > 0)
            {
                if (fCMax == fR)
                {
                    fH = 60 * (((fG - fB) / fDelta) % 6);
                }
                else if (fCMax == fG)
                {
                    fH = 60 * (((fB - fR) / fDelta) + 2);
                }
                else// if (fCMax == fB)
                {
                    fH = 60 * (((fR - fG) / fDelta) + 4);
                }

                if (fCMax > 0)
                {
                    fS = fDelta / fCMax;
                }
                else
                {
                    fS = 0;
                }

                fV = fCMax;
            }
            else
            {
                fH = 0;
                fS = 0;
                fV = fCMax;
            }

            if (fH < 0)
            {
                fH = 360 + fH;
            }
        }


        /*! \brief Convert HSV to RGB color space

          Converts a given set of HSV values `h', `s', `v' into RGB
          coordinates. The output RGB values are in the range [0, 1], and
          the input HSV values are in the ranges h = [0, 360], and s, v =
          [0, 1], respectively.

          \param fR Red component, used as output, range: [0, 1]
          \param fG Green component, used as output, range: [0, 1]
          \param fB Blue component, used as output, range: [0, 1]
          \param fH Hue component, used as input, range: [0, 360]
          \param fS Hue component, used as input, range: [0, 1]
          \param fV Hue component, used as input, range: [0, 1]

        */
        private static float fabs(float value)
        {
            return (value > 0.0f ? value : -value);
        }

        public static void HSVtoRGB(out float fR, out float fG, out float fB, float fH, float fS, float fV)
        {
            float fC = fV * fS; // Chroma
            float fHPrime = (fH / 60.0f) %  6;
            float fX = fC * (1.0f - fabs((fHPrime % 2.0f) - 1.0f));
            float fM = fV - fC;

            if (0 <= fHPrime && fHPrime < 1)
            {
                fR = fC;
                fG = fX;
                fB = 0;
            }
            else if (1 <= fHPrime && fHPrime < 2)
            {
                fR = fX;
                fG = fC;
                fB = 0;
            }
            else if (2 <= fHPrime && fHPrime < 3)
            {
                fR = 0;
                fG = fC;
                fB = fX;
            }
            else if (3 <= fHPrime && fHPrime < 4)
            {
                fR = 0;
                fG = fX;
                fB = fC;
            }
            else if (4 <= fHPrime && fHPrime < 5)
            {
                fR = fX;
                fG = 0;
                fB = fC;
            }
            else if (5 <= fHPrime && fHPrime < 6)
            {
                fR = fC;
                fG = 0;
                fB = fX;
            }
            else
            {
                fR = 0;
                fG = 0;
                fB = 0;
            }

            fR += fM;
            fG += fM;
            fB += fM;
        }

        public static Image getJpgThumbnailFromFile(string nefName)
        {
            string jpgFile = thumbFilenameFromNef(nefName);
            if (!File.Exists(jpgFile)) return null;
            return Image.FromFile(jpgFile);
        }

        private static string thumbFilenameFromNef(string nefName)
        {
            return Path.GetDirectoryName(nefName) + "\\" +
                                 Path.GetFileNameWithoutExtension(nefName) + "_thumb.jpg";
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static void saveJpgThumbnailToFile(Image image, string filename)
        {
            Bitmap myBitmap = new Bitmap(image);
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            string jpgFileName = Path.GetDirectoryName(filename) + "\\" +
                                 Path.GetFileNameWithoutExtension(filename) + "_thumb.jpg";

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.

            // An EncoderParameters object has an array of EncoderParameter

            // objects. In this case, there is only one

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with quality level 90.
            myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myBitmap.Save(jpgFileName, myImageCodecInfo, myEncoderParameters);
        }

        public static Image getJpgThumbnailFromNEF(string filename)
        {
            byte[] imgData = extractJpegFromRaw(filename);
            if (imgData != null)
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(imgData, 0, imgData.Length);
                Image myImage = Image.FromStream(stream);
                Bitmap resizedImage = ResizeImage(myImage, THUMB_WIDTH, THUMB_HEIGHT);
                Rectangle rect = new Rectangle(0, 0, resizedImage.Width, resizedImage.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    resizedImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                          resizedImage.PixelFormat);
                IntPtr ptr = bmpData.Scan0;

                int bytes = Math.Abs(bmpData.Stride) * resizedImage.Height;
                byte[] rgbValues = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                // Find min max values
                byte min = 0xFF;
                byte max = 0;
                int[] totalLumin = new int[] { 0, 0, 0 };
                for (int i = 0; i < bytes; i += 4)
                {
                    if (rgbValues[i] < min) min = rgbValues[i];
                    if (rgbValues[i] > max) max = rgbValues[i];
                    if (rgbValues[i + 1] < min) min = rgbValues[i + 1];
                    if (rgbValues[i + 1] > max) max = rgbValues[i + 1];
                    if (rgbValues[i + 1] < min) min = rgbValues[i + 1];
                    if (rgbValues[i + 1] > max) max = rgbValues[i + 1];
                    totalLumin[0] += (int)rgbValues[i + 0];
                    totalLumin[1] += (int)rgbValues[i + 1];
                    totalLumin[2] += (int)rgbValues[i + 2];
                }
                for (int i = 0; i < 3; i++) totalLumin[i] /= (bytes / 4);
                int totalAverage = (totalLumin[0] + totalLumin[1] + totalLumin[2]) / 3;
                float grayContrastFactor = 127.0f / (float)totalAverage;
                byte[] contrastLUT = new byte[256];
                for (int i = 0; i < totalAverage; i++)
                {
                    contrastLUT[i] = (byte)((float)i * (grayContrastFactor * (float)i / (float)totalAverage + 1.0f * (float)(totalAverage - i) / totalAverage));
                }
                for (int i = totalAverage; i < 256; i++)
                {
                    contrastLUT[i] = (byte)((float)i * (grayContrastFactor * (float)(255 - i) / (float)(255 - totalAverage) + 1.0f * (float)(i - totalAverage) / (float)(255 - totalAverage)));
                }

                // Perform level adjustment
                float r, g, b, h, s, v;
                for (int i = 0; i < bytes; i += 4)
                {
                    //if ((i % 2400) < 1200)
                    {
                        r = (float)rgbValues[i + 0] / 255.0f;
                        g = (float)rgbValues[i + 1] / 255.0f;
                        b = (float)rgbValues[i + 2] / 255.0f;
                        ImageProcessing.RGBtoHSV(r, g, b, out h, out s, out v);
                        v = (v * grayContrastFactor > 1.0f ? 1.0f : (v * grayContrastFactor));
                        ImageProcessing.HSVtoRGB(out r, out g, out b, h, s, v);
                        rgbValues[i + 0] = (byte)(r * 255.0f);
                        rgbValues[i + 1] = (byte)(g * 255.0f);
                        rgbValues[i + 2] = (byte)(b * 255.0f);

                    }
                }

                // Copy the RGB values back to the bitmap
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                resizedImage.UnlockBits(bmpData);

                return resizedImage;
            }
            else return null;
        }
    }
}
