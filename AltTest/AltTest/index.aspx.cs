using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Web.Services;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;
using System.Threading;

namespace AltTest
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        
        [WebMethod()]
        public static string ProcessImage(string url, string dropboxValue)
        {
            Bitmap mainBitmap;
            byte[] byteArrayFromImageUrl;
            using (WebClient client = new WebClient())
            {
                byteArrayFromImageUrl = client.DownloadData(url);
            }
            using (MemoryStream ms = new MemoryStream(byteArrayFromImageUrl))
            {
                mainBitmap = new Bitmap(ms);
            }
            
            if (dropboxValue == "GrayValue")
                ToGrayProcessImage(mainBitmap);
            if (dropboxValue == "SwapGandBValue")
                SwapGandBProcessImage(mainBitmap);

            byte[] byteArrayOfResultImage;
            using (MemoryStream ms2 = new MemoryStream())
            {
                mainBitmap.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
                byteArrayOfResultImage = ms2.ToArray();
            }
            Thread.Sleep(5000);
            return Convert.ToBase64String(byteArrayOfResultImage);
        }

        private static void ToGrayProcessImage(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    byte grayColor = (byte)(.299 * pixelColor.R + .587 * pixelColor.G + .114 * pixelColor.B);
                    bitmap.SetPixel(x, y, Color.FromArgb(grayColor, grayColor, grayColor));
                }
            }
        }

        private static void SwapGandBProcessImage(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    bitmap.SetPixel(x, y, Color.FromArgb(pixelColor.R, pixelColor.B, pixelColor.G));
                }
            }
        }
    }
}