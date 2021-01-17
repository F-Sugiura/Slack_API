using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;


namespace Slack_API
{
    public class ChatPostMessage
    {

        public static void CreateTxt()
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
                parameters["channel"] = channel;
                parameters["text"] = "TEST";

            var client = new WebClient();
                client.QueryString = parameters;
                client.Headers.Add("Content-Type", "application/json");

            byte[] responseBytes = Encoding.UTF8.GetBytes
                (
                    client.UploadString
                    (
                        "https://slack.com/api/chat.postMessage",
                        "POST"
                    )
                );

                //result
                String responseString = Encoding.UTF8.GetString(responseBytes);

                ReturnValue Return =
                    JsonConvert.DeserializeObject<ReturnValue>(responseString);

                Console.WriteLine(Return);

            }

        }   
}






