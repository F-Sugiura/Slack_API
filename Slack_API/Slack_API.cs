using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Collections.Specialized;

namespace Slack_API
{
    public static class Slack_API
    {
        [FunctionName("Slack_API")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            Slack();

            return new OkObjectResult(responseMessage);
        }

        public static void Slack()
        {
            
            try
            {
                //Create　Text
                string path = Directory.GetCurrentDirectory() + "/Test.txt";

                using (FileStream fs = File.Create(path))
                {
                    byte[] contents = new UTF8Encoding(true).GetBytes("Test");
                    // Add some information to the file.
                    fs.Write(contents, 0, contents.Length);
                }

                PostSlackFile(path, path);

                //Delete Text
                File.Delete(path);
               
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        class ReturnValue
        {
            public bool ok { get; set; }
            public String error { get; set; }
            public SlackFile file { get; set; }
        }
        
        class SlackFile
        {
            public String id { get; set; }
            public String name { get; set; }
        }

        public static void PostSlackFile(string Filename, string txt)
        {
            string channel = System.Environment.GetEnvironmentVariable("Channel");
            string token = System.Environment.GetEnvironmentVariable("Token");


            var parameters = new NameValueCollection();

            // token & ChannelID
            parameters["token"] = token;
            parameters["channels"] = channel;

            var client = new WebClient();
            client.QueryString = parameters;
            byte[] responseBytes = client.UploadFile(
                    "https://slack.com/api/files.upload",
                    Filename
            );

            //result
            String responseString = Encoding.UTF8.GetString(responseBytes);

            ReturnValue Return =
                JsonConvert.DeserializeObject<ReturnValue>(responseString);

            Console.WriteLine(Return);

        }

    }
}





 