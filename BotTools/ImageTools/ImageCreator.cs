using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using System.IO;

namespace MobieBotVK.BotTools.ImageTools
{
    public static class ImageCreator
    {
        public static string TempNewFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tmpnewfile.jpg");
        public static string TempDownFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tmpdownfile.jpg");


        public static bool DeleteColor(params string[] rgb)
        {
            if (File.Exists(TempDownFile))
            {
                byte dr = 1, dg = 1, db = 1;
                foreach (string s in rgb)
                {
                    string tmp = s.ToLower();
                    if (tmp == "r")
                        dr = 0;
                    else if (tmp == "g")
                        dg = 0;
                    else if (tmp == "b")
                        db = 0;
                }
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        bmp.SetPixel(i, j, new SKColor((byte)(color.Red * dr), (byte)(color.Green * dg), (byte)(color.Blue * db)));
                    }

                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool MaxColor(params string[] rgb)
        {
            if (File.Exists(TempDownFile))
            {
                byte dr = 1, dg = 1, db = 1;
                foreach (string s in rgb)
                {
                    string tmp = s.ToLower();
                    if (tmp == "r")
                        dr = 0;
                    else if (tmp == "g")
                        dg = 0;
                    else if (tmp == "b")
                        db = 0;
                }
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        bmp.SetPixel(i, j,
                            new SKColor((dr == 0) ? (byte)255 : color.Red,
                                        (dg == 0) ? (byte)255 : color.Green,
                                        (db == 0) ? (byte)255 : color.Blue));
                    }

                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool Negative()
        {
            if (File.Exists(TempDownFile))
            {
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        bmp.SetPixel(i, j, new SKColor((byte)(255 - color.Red), (byte)(255 - color.Green), (byte)(255 - color.Blue)));
                    }
                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool LowDeepColor(int param)
        {
            if (File.Exists(TempDownFile))
            {
                if (param <= 0 || param >= 255)
                    param = 1;
                double kef = 255d / param;
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        byte r = (byte)(Math.Round(Math.Round((color.Red / 255d) * param) * kef));
                        byte g = (byte)(Math.Round(Math.Round((color.Green / 255d) * param) * kef));
                        byte b = (byte)(Math.Round(Math.Round((color.Blue / 255d) * param) * kef));
                        bmp.SetPixel(i, j, new SKColor(r, g, b));
                    }

                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool OnlyBlackWhite()
        {
            if (File.Exists(TempDownFile))
            {
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        if ((color.Red + color.Green + color.Blue) / 3 > 127)
                            bmp.SetPixel(i, j, new SKColor(255, 255, 255));
                        else
                            bmp.SetPixel(i, j, new SKColor(0, 0, 0));
                    }
                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool BlackWhite()
        {
            if (File.Exists(TempDownFile))
            {
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        byte tmp = (byte)((color.Red + color.Green + color.Blue) / 3);
                        bmp.SetPixel(i, j, new SKColor(tmp, tmp, tmp));
                    }
                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool Brightness(int param)
        {
            if (File.Exists(TempDownFile))
            {
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                if (param < 0 || param > 100)
                    param = 0;
                double kef = 1 + param / 100d;
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        long tmpr = (long)(color.Red * kef);
                        long tmpg = (long)(color.Green * kef);
                        long tmpb = (long)(color.Blue * kef);
                        if (tmpr > 255) tmpr = 255; if (tmpr < 0) tmpr = 0;
                        if (tmpg > 255) tmpg = 255; if (tmpg < 0) tmpg = 0;
                        if (tmpb > 255) tmpb = 255; if (tmpb < 0) tmpb = 0;
                        bmp.SetPixel(i, j, new SKColor((byte)tmpr, (byte)tmpg, (byte)tmpb));
                    }
                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool Contrast(int param)
        {
            if (File.Exists(TempDownFile))
            {
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                if (param < 0 || param > 100)
                    param = 0;
                double kef = 1 + param / 100d;
                long sum = 0;
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        sum += color.Red + color.Green + color.Blue;
                    }
                sum /= bmp.Width * bmp.Height;

                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        long tmpr = (long)(sum + kef * (color.Red - sum));
                        long tmpg = (long)(sum + kef * (color.Green - sum));
                        long tmpb = (long)(sum + kef * (color.Blue - sum));
                        if (tmpr > 255) tmpr = 255; if (tmpr < 0) tmpr = 0;
                        if (tmpg > 255) tmpg = 255; if (tmpg < 0) tmpg = 0;
                        if (tmpb > 255) tmpb = 255; if (tmpb < 0) tmpb = 0;

                        bmp.SetPixel(i, j, new SKColor((byte)tmpr, (byte)tmpg, (byte)tmpb));
                    }

                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool SwapColor(string from, string to)
        {
            if (File.Exists(TempDownFile))
            {
                byte[] file = File.ReadAllBytes(TempDownFile);
                SKBitmap bmp = SKBitmap.Decode(file);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        SKColor color = bmp.GetPixel(i, j);
                        byte r, g, b;
                        if ((from == "r" && to == "g") || (from == "g" && to == "r"))
                        {
                            r = color.Green;
                            g = color.Red;
                            b = color.Blue;
                        }
                        else if ((from == "r" && to == "b") || (from == "b" && to == "r"))
                        {
                            r = color.Blue;
                            g = color.Green;
                            b = color.Red;
                        }
                        else if ((from == "b" && to == "g") || (from == "g" && to == "b"))
                        {
                            r = color.Red;
                            g = color.Blue;
                            b = color.Green;
                        }
                        else
                        {
                            r = color.Red;
                            g = color.Green;
                            b = color.Blue;
                        }

                        bmp.SetPixel(i, j, new SKColor(r, g, b));
                    }
                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

        public static bool QuoteForUser(string user, string text)
        {
            if (File.Exists(TempDownFile))
            {
                //byte[] file = File.ReadAllBytes(TempDownFile);
                //SKBitmap bmp = SKBitmap.Decode(file);
                SKBitmap output = new SKBitmap(600, 250);
                for (int i = 0; i < output.Width; i++)
                    for (int j = 0; j < output.Height; j++)
                    {
                        SKColor color = output.GetPixel(i, j);
                        output.SetPixel(i, j, new SKColor(0, 0, 0));
                    }

                // Create bitmap and draw on it
                using (SKPaint textPaint = new SKPaint { TextSize = 32 })
                {
                    SKRect bounds = new SKRect();
                    textPaint.MeasureText(text, ref bounds);
                    textPaint.Color = new SKColor(255, 0, 0);

                    //output = new SKBitmap((int)bounds.Right, (int)bounds.Height);

                    using (SKCanvas bitmapCanvas = new SKCanvas(output))
                    {
                        bitmapCanvas.Clear();
                        bitmapCanvas.DrawText(text, 50, 50, textPaint);
                    }
                }


                using (Stream s = File.OpenWrite(TempNewFile))
                {
                    SKData data = SKImage.FromBitmap(output).Encode(SKEncodedImageFormat.Jpeg, 100);
                    data.SaveTo(s);
                }
                File.Delete(TempDownFile);
                return true;
            }
            return false;
        }

    }
}
