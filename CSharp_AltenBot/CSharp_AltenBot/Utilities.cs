using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net;
using System.ComponentModel;


namespace CSharp_AltenBot
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        //This one was only meant for gd
        public static string SendPostRequest(string Url, string Parameters)
        {
            WebRequest request = WebRequest.Create(Url);
            request.Method = "POST";
            string postData = Parameters;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            Console.WriteLine($"Url: {Url}\nParameters: {Parameters}");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            Console.WriteLine(responseFromServer);
            return responseFromServer;
        }

        public static string[] GetLevelPacket(string name, string p = "0")
        {
            string Params = $"gameVersion=21&binaryVersion=35&gdw=0&type=0&str={name}&page={p}&secret=Wmfd2893gb7";
            string Packet = SendPostRequest("http://boomlings.com/database/getGJLevels21.php", Params);
            string[] LevelPacket = Packet.Replace('~', ' ').Split('|', ':');
            return LevelPacket;
        }


    public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";
        }
    }
}
