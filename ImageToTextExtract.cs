using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// install NuGet Pack >>> ProjectOxford.Vision

namespace ComputerVision
{
    class ImageToTextRecognitionExtract
    {
        const string API_key = "Your API key";
        const string API_location = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0";


        static void Main(string[] args)
        {
            string imgToAnalyze = @"C:\Users\PM-ADMIN\Source\Repos\ComputerVision\images\Famous.png";
            //string imgToAnalyze = @"C:\Users\PM-ADMIN\Source\Repos\ComputerVision\images\Receipt.png";
            //TextExtraction(imgToAnalyze, true);
            TextExtraction(imgToAnalyze, false);
            Console.ReadLine();
        }


        public static void PrintResults(string[] res)
        {
            foreach (string r in res)
            {
                Console.WriteLine(r);
            }
        }


        public static void TextExtraction(string fname, bool wrds)
        {
            Task.Run(async () =>
            {
                string[] res = await TextExtractionCore(fname, wrds);
                PrintResults(res);
            }).Wait();
        }


        public static async Task<string[]> TextExtractionCore(string fname, bool wrds)
        {
            VisionServiceClient client = new VisionServiceClient(API_key, API_location);
            string[] textres = null;
            if (File.Exists(fname))
            {
                using (Stream stream = File.OpenRead(fname))
                {
                    OcrResults res = await client.RecognizeTextAsync(stream, "unk", false);
                    textres = GetExtracted(res, wrds);
                }
            }
            return textres;
        }


        public static string[] GetExtracted(OcrResults res, bool wrds)
        {
            List<string> items = new List<string>();
            foreach (Region r in res.Regions)
                foreach (Line l in r.Lines)
                    if (wrds)
                        items.AddRange(GetWords(l));
                    else
                    {
                        items.Add(GetLineAsString(l));
                    }

            return items.ToArray();
        }


        public static List<string> GetWords(Line line)
        {
            List<string> words = new List<string>();
            foreach (Word w in line.Words)
                words.Add(w.Text);
            return words;
        }


        public static string GetLineAsString(Line line)
        {
            List<string> words = GetWords(line);
            return words.Count > 0 ? string.Join(" ", words) : string.Empty;
        }
    }
}
