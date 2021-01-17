using System;
using System.IO;
using System.Text;

namespace Slack_API
{
    public class CreateMsg
    {
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

                Slack_APIFilesUpload Class = new Slack_APIFilesUpload();

                Class.PostSlackFile(path, path);
                //Delete Text
                File.Delete(path);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
