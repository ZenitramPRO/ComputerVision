using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JsonPrettyPrinterPlus;

namespace ComputerVision
{
    class ThumbnailGeneration
    {
        const string subscriptionKey = "Your subscriptionKey";
        const string uriBase = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0/generateThumbnail";

        static void Main(string[] args)
        {
            string imageFilePath = @"C:\Users\PM-ADMIN\Source\Repos\ComputerVision\images\Stars.png";
            GenerateThumbnail(imageFilePath, 80,80,true);
            Console.ReadLine();
        }

        public static async void GenerateThumbnail(string imageFilePath, int width, int height, bool smart)
        {
            byte[] thumbnail = await GetThumbnail(imageFilePath, width, height, smart);
            string thumbnailFullPath = string.Format("{0}\\thumbnail_{1:yyyy-MMM-dd_hh-mm-ss}.jpg",
                Path.GetDirectoryName(imageFilePath), DateTime.Now);
            using (BinaryWriter bw = new BinaryWriter(new FileStream(thumbnailFullPath, FileMode.OpenOrCreate, FileAccess.Write)))
                bw.Write(thumbnail);
        }

        public static async Task<byte[]> GetThumbnail(string imageFilePath, int width, int height, bool smart)
        { 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = $"width={width.ToString()}&height={height.ToString()}&smartCropping={smart.ToString().ToLower()}";
            string uri = uriBase + "?" + requestParameters;
            HttpResponseMessage response = null;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                return await response.Content.ReadAsByteArrayAsync();
            }
        } 

        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
         
    }
}
