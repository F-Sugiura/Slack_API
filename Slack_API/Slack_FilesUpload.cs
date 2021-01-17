using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Slack_API
{
    public  class Slack_APIFilesUpload
    {
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

        public void PostSlackFile(string Filename, string txt)
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





 