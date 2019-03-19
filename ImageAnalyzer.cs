using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsonPrettyPrinterPlus;

namespace ComputerVision
{
    class ImageAnalyzer
    {
        const string skey = "Your subscriptionKey";
        const string uriBase = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0/analyze";

        static void Main(string[] args)
        {
            string imageFilePath = @"C:\Users\PM-ADMIN\Source\Repos\ComputerVision\images\Rally Car.png";
            MakeAnalysisRequest(imageFilePath);
            Console.ReadLine();
        }

        public static async void MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", skey);

            string requestParameters = "visualFeatures=Categories, Description, Color&language=en";
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response = null;
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                string contentstring = await response.Content.ReadAsStringAsync();
                Console.WriteLine("\nResponse:\n");
                Console.WriteLine(contentstring.PrettyPrintJson());
                //Console.WriteLine(JsonPrettyPrint(contentstring));
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
