using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;

// Install the ff: NuGet package 1. ProjectOxford.Vision (manual on package manager console >>> Install-Package Microsoft.ProjectOxford.Vision -Version 1.0.370)

namespace ComputerVision
{
    class SmartThumbnailGeneration
    {
        const string API_key = "Your API key";
        const string API_location = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0";

        static void Main(string[] args)
        {
            string imgToAnalyze = @"C:\Users\PM-ADMIN\Source\Repos\ComputerVision\images\Man.png";
            SmartThumbnail(imgToAnalyze, 80, 80, true);
            SmartThumbnail(imgToAnalyze, 180, 180, false);
            Console.ReadKey();
        }


        public static void SmartThumbnail(string fname, int width, int height, bool smartCropping)
        {
            Task.Run(async () =>
            {
                string imgName = Path.GetFileName(fname);
                Console.WriteLine($"Thumbnail image for: {imgName}");
                byte[] thumbnail = await SmartThumbnailGen(fname, width, height, smartCropping);
                string thumbnailFullPath = string.Format("{0}\\thumbnail_{1:yyyy-MMM-dd_hh-mm-ss}.jpg",
                    Path.GetDirectoryName(fname), DateTime.Now);
                using (BinaryWriter bw = new BinaryWriter(new FileStream(thumbnailFullPath, FileMode.OpenOrCreate, FileAccess.Write)))
                    bw.Write(thumbnail);
            }).Wait();
        }



        public static async Task<byte[]> SmartThumbnailGen(string fname, int width, int height, bool smartCropping)
        {
            byte[] thumbnail = null;
            VisionServiceClient client = new VisionServiceClient(API_key, API_location);

            if (File.Exists(fname))
                using (Stream stream = File.OpenRead(fname))
                    thumbnail = await client.GetThumbnailAsync(stream, width, height, smartCropping);
            return thumbnail;
        }
    }
}
