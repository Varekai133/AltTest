using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Web.Services;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AltTest
{
    public partial class index : System.Web.UI.Page
    {
        private static List<CancellationTokenSource> cancellationTokenSourceList = new List<CancellationTokenSource>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod()]
        public static string ProcessImage(string imageUrlInserterByUser, string dropboxValue)
        {
            string urlImageToSendToUser = null;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSourceList.Add(cancellationTokenSource);
            if (cancellationTokenSourceList.Count != 1)
                cancellationTokenSourceList[cancellationTokenSourceList.Count - 2].Cancel();
            var currentTask = Task.Run(() =>
            {
                try
                {
                    urlImageToSendToUser  = ProcessCurrentImage(cancellationTokenSource.Token, imageUrlInserterByUser, dropboxValue);
                }
                catch(WebException)
                {

                }
                catch (OperationCanceledException)
                {
                    
                }
                finally
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSourceList.Remove(cancellationTokenSource);
                }
            }
            , cancellationTokenSource.Token);
            currentTask.Wait();
            return urlImageToSendToUser;
        }
        
        private static string ProcessCurrentImage(CancellationToken cancellationToken, string imageUrlInserterByUser, string dropboxValue)
        {            
            Bitmap mainBitmap;
            byte[] byteArrayFromImageUrl;
            using (WebClient client = new WebClient())
            {
                byteArrayFromImageUrl = client.DownloadData(imageUrlInserterByUser);
            }
            using (MemoryStream memoryStreamToCreateBitmapFromUrl = new MemoryStream(byteArrayFromImageUrl))
            {
                mainBitmap = new Bitmap(memoryStreamToCreateBitmapFromUrl);
            }

            if (dropboxValue == "GrayValue")
                ToGrayProcessImage(mainBitmap, cancellationToken);
            if (dropboxValue == "SwapGandBValue")
                SwapGandBProcessImage(mainBitmap, cancellationToken);

            byte[] byteArrayOfResultImage;
            using (MemoryStream memoryStreamToSaveBitmapToImage = new MemoryStream())
            {
                mainBitmap.Save(memoryStreamToSaveBitmapToImage, System.Drawing.Imaging.ImageFormat.Png);
                byteArrayOfResultImage = memoryStreamToSaveBitmapToImage.ToArray();
            }
            return Convert.ToBase64String(byteArrayOfResultImage);
        }

        private static void ToGrayProcessImage(Bitmap bitmap, CancellationToken cancellationToken)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                Thread.Sleep(10);
                for (int y = 0; y < bitmap.Height; y++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Color pixelColor = bitmap.GetPixel(x, y);
                    byte grayColorFromRGB = (byte)(.299 * pixelColor.R + .587 * pixelColor.G + .114 * pixelColor.B);
                    bitmap.SetPixel(x, y, Color.FromArgb(grayColorFromRGB, grayColorFromRGB, grayColorFromRGB));
                }
            }
        }

        private static void SwapGandBProcessImage(Bitmap bitmap, CancellationToken cancellationToken)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                Thread.Sleep(10);
                for (int y = 0; y < bitmap.Height; y++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Color pixelColor = bitmap.GetPixel(x, y);
                    bitmap.SetPixel(x, y, Color.FromArgb(pixelColor.R, pixelColor.B, pixelColor.G));
                }
            }
        }
    }
}
