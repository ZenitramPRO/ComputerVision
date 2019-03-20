using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;


// add NuGet Pack >>> ProjectOxford.Vision

namespace ComputerVision
{
    class HandwritingImageToTextExtraction
    {
        const string API_key = "Your API key";
        const string API_location = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0";


        static void Main(string[] args)
        {
            string imgToAnalyze = @"C:\Users\PM-ADMIN\Source\Repos\ComputerVision\images\Handwriting.png";
            HandwritingExtraction(imgToAnalyze, false);
            Console.ReadLine();
        }


        public static void HandwritingExtraction(string fname, bool wrds)
        {
            Task.Run(async () =>
            {
                string[] res = await HandwritingExtractionCore(fname, wrds);
                PrintResults(res);
            }).Wait();
        }


        public static async Task<string[]> HandwritingExtractionCore(string fname, bool wrds)
        {
            VisionServiceClient client = new VisionServiceClient(API_key, API_location);
            string[] textres = null;
            if (File.Exists(fname))
            {
                using (Stream stream = File.OpenRead(fname))
                {
                    HandwritingRecognitionOperation op = await client.CreateHandwritingRecognitionOperationAsync(stream);
                    HandwritingRecognitionOperationResult res = await client.GetHandwritingRecognitionOperationResultAsync(op);
                    textres = GetExtracted(res, wrds);
                }
            }
            return textres;
        }


        public static string[] GetExtracted(HandwritingRecognitionOperationResult res, bool wrds)
        {
            List<string> items = new List<string>();
            foreach (HandwritingTextLine l in res.RecognitionResult.Lines)
                if (wrds)
                    items.AddRange(GetWords(l));
                else
                {
                    items.Add(GetLineAsString(l));
                }

            return items.ToArray();
        }


        public static void PrintResults(string[] res)
        {
            foreach (string r in res)
            {
                Console.WriteLine(r);
            }
        }


        public static List<string> GetWords(HandwritingTextLine line)
        {
            List<string> words = new List<string>();
            foreach (HandwritingTextWord w in line.Words)
                words.Add(w.Text);
            return words;
        }


        public static string GetLineAsString(HandwritingTextLine line)
        {
            List<string> words = GetWords(line);
            return words.Count > 0 ? string.Join(" ", words) : string.Empty;
        }

    }
}
