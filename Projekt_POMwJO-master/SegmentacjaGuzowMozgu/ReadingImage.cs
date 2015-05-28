using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace SegmentacjaGuzowMozgu
{
    class ReadingImage
    {
        #region private members
        itk.simple.Image image;
        Bitmap[] bitmaps;
        #endregion
        // Siema siema, pierwszy commit
        #region constructors
        public ReadingImage() { }
        public ReadingImage(itk.simple.Image img) { this.image = img; }
        #endregion

        #region accessors
        public Bitmap[] Bitmaps
        {
            get { return bitmaps; }
        }
        public itk.simple.Image Image
        {
            get { return image; }
            set { image = value; }
        }
        #endregion

        #region methods

        IntPtr GetPixelType(itk.simple.Image image)
        {
            string pixelType=image.GetPixelIDTypeAsString();
            if (pixelType.Contains("8-bit unsigned integer")) { return image.GetBufferAsUInt8(); }
            else if (pixelType.Contains("16-bit unsigned integer")) { return image.GetBufferAsUInt16(); }
            else if (pixelType.Contains("32-bit unsigned integer")) { return image.GetBufferAsUInt32(); }
            //if (pixelType.Contains("64-bit unsigned integer")) { return image.GetBufferAsUInt64(); }
            else if (pixelType.Contains("8-bit signed integer")) { return image.GetBufferAsInt8(); }
            else if (pixelType.Contains("16-bit signed integer")) { return image.GetBufferAsInt16(); }
            else if (pixelType.Contains("32-bit signed integer")) { return image.GetBufferAsInt32(); }
            //if (pixelType.Contains("64-bit signed integer")) { return image.GetBufferAsInt64(); }
            else if (pixelType.Contains("float")) { return image.GetBufferAsFloat(); }
            else { return image.GetBufferAsDouble(); }
        }
        public Bitmap[] CreateBitmapLayer(itk.simple.Image image)
        {
            // przekonwertuj teraz na bitmapę C#
            uint cols = image.GetSize()[0];
            uint rows = image.GetSize()[1];
            uint layers = image.GetDepth();

            // wartość zwracana - tyle obrazków, ile warstw
            System.Drawing.Bitmap[] ret;
            if (layers == 0)
            {
                ret = new System.Drawing.Bitmap[1];
                ret[0] = CreateBitmap(image);
            return ret;
            }
            else
            {
                ret = new System.Drawing.Bitmap[layers];
                unsafe
                {
                    byte* buffer = (byte*)GetPixelType(image);

                    // w strumieniu na każdy piksel 2 bajty; tutaj LittleEndian (mnie znaczący bajt wcześniej)
                    for (uint l = 0; l < layers; l++)
                    {
                        System.Drawing.Bitmap X = new System.Drawing.Bitmap((int)cols, (int)rows);
                        double[,] Y = new double[cols, rows];
                        double m = 0;

                        for (int r = 0; r < rows; r++)
                            for (int c = 0; c < cols; c++)
                            {
                                // współrzędne w strumieniu
                                int j = ((int)(l * rows * cols) + (int)(r * cols) + (int)c) * 2;
                                Y[r, c] = (double)buffer[j + 1] * 256 + (double)buffer[j];
                                // przeskalujemy potem do wartości max.
                                if (Y[r, c] > m)
                                    m = Y[r, c];
                            }

                        // wolniejsza metoda tworzenia bitmapy
                        for (int r = 0; r < rows; r++)
                            for (int c = 0; c < cols; c++)
                            {
                                int f = (int)(255 * (Y[r, c] / m));
                                X.SetPixel(c, r, System.Drawing.Color.FromArgb(f, f, f));
                            }
                        // kolejna bitmapa
                        ret[l] = X;
                    }

                }
                return ret;
            }
        }

        Bitmap CreateBitmap(itk.simple.Image image)
        {
            Bitmap bitmap;
            itk.simple.PixelIDValueEnum v=image.GetPixelID();
            //PixelFormat format = PixelFormat.Format8bppIndexed;
            PixelFormat format = PixelFormat.Format8bppIndexed;
            // Check if the stride is the same as the width
            if (image.GetSize()[0] % 4 == 0)
            {
                // Width = Stride: simply use the Bitmap constructor
                bitmap = new Bitmap((int)image.GetSize()[0],  // Width
                                    (int)image.GetSize()[1],  // Height
                                    (int)image.GetSize()[0],  // Stride
                                    format,         // PixelFormat
                                    GetPixelType(image)   // Buffer
                                    );
            }
            else
            {

                unsafe
                {
                    // Width != Stride: copy data from buffer to bitmap
                    int width = (int)image.GetSize()[0];
                    int height = (int)image.GetSize()[1];
                    byte* buffer = (byte*)image.GetBufferAsUInt8().ToPointer();

                    // Compute the stride
                    int stride = width;
                    if (width % 4 != 0)
                        stride = ((width / 4) * 4 + 4);

                    bitmap = new Bitmap(width, height, format);
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, format);

                    for (int j = 0; j < height; j++)                          // Row
                    {
                        byte* row = (byte*)bitmapData.Scan0 + (j * stride);
                        for (int i = 0; i < width; i++)                       // Column
                            row[i] = buffer[(j * width) + i];
                    }
                    bitmap.UnlockBits(bitmapData);
                }// end unsafe
            }
                bitmap.Palette = this.CreateGrayscalePalette(format, 256);
                return bitmap;
            
        }

        ColorPalette CreateGrayscalePalette(PixelFormat format, int numberOfEntries)
        {
            ColorPalette palette;    // The Palette we are stealing
            Bitmap bitmap;           // The source of the stolen palette

            // Make a new Bitmap object to steal its Palette
            bitmap = new Bitmap(1, 1, format);

            palette = bitmap.Palette;   // Grab the palette
            bitmap.Dispose();           // Cleanup the source Bitmap

            // Populate the palette
            for (int i = 0; i < numberOfEntries; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);

            // Return the palette
            return palette;
        }

        public BitmapImage ToWpfBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
        #endregion
    }
        
    
}
