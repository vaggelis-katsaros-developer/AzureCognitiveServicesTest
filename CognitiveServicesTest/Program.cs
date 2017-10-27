using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Web;

namespace CognitiveServicesTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> resultDescribe = DescribeImage(@"C:\Users\katsaros\Downloads\dog.jpg");
            Task<string> resultText = ExtractText(@"C:\Users\katsaros\Downloads\next.jpg");
            Console.WriteLine(resultDescribe.Result);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(resultText.Result);
            Console.ReadLine();
        }

        public static async Task<string> DescribeImage(string imageFilePath)
        {
            using (HttpClient hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",
                ConfigurationManager.AppSettings["AzureSubscriptionKeyVision"]);

                using (MultipartFormDataContent reqContent = new MultipartFormDataContent())
                {
                    var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/describe";

                    try
                    {
                        var imgContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imageFilePath));
                        reqContent.Add(imgContent);
                        HttpResponseMessage resp = await hc.PostAsync(uri, reqContent);
                        string respJson = await resp.Content.ReadAsStringAsync();
                        return respJson;
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        return "The specified image file path is invalid.";
                    }
                    catch (ArgumentException ex)
                    {
                        return "The HTTP request object does not seem to be correctly formed.";
                    }
                }
            }
        }

        public static async Task<string> ExtractText(string imageFilePath)
        {
            using (HttpClient hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",
                ConfigurationManager.AppSettings["AzureSubscriptionKeyVision"]);
                using (MultipartFormDataContent reqContent = new MultipartFormDataContent())
                {
                    var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/ocr";

                    try
                    {
                        var imgContent = new ByteArrayContent(System.IO.File.
                        ReadAllBytes(imageFilePath));
                        reqContent.Add(imgContent);
                        HttpResponseMessage resp = await hc.PostAsync(uri, reqContent);
                        string respJson = await resp.Content.ReadAsStringAsync();
                        return respJson;
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        return "The specified image file path is invalid.";
                    }
                    catch (ArgumentException ex)
                    {
                        return "The HTTP request object does not seem to be correctly formed.";
                    }
                }
            }
        }
    }
}
