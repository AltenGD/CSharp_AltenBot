using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;

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
            return responseFromServer;
        }


        //fuck this is so bad
        public static string[] GetLevelPacket(string Level)
        {
            int value;
            if (int.TryParse(Level, out value))
            {
                string Params = $"gameVersion=21&binaryVersion=35&gdw=0&type=0&str={Level}&diff=-&len=-&page=0&total=0&uncompleted=0&onlyCompleted=0&featured=0&original=0&twoPlayer=0&coins=0&epic=0&secret=Wmfd2893gb7";
                string Packet = SendPostRequest("http://boomlings.com/database/getGJLevels21.php", Params);
                string[] LevelPacket = Packet.Replace('~', ' ').Split('|', ':');
                return LevelPacket;
            }
            else
            {
                string Paramss = $"gameVersion=21&binaryVersion=35&gdw=0&type=0&str={Level}&diff=-&len=-&page=0&total=0&uncompleted=0&onlyCompleted=0&featured=0&original=0&twoPlayer=0&coins=0&epic=0&secret=Wmfd2893gb7";
                string Packetss = SendPostRequest("http://boomlings.com/database/getGJLevels21.php", Paramss);
                string[] LevelPacketss = Packetss.Split(':');
                string LevelID = LevelPacketss[1];
                string Params = $"gameVersion=21&binaryVersion=35&gdw=0&type=0&str={LevelID}&diff=-&len=-&page=0&total=0&uncompleted=0&onlyCompleted=0&featured=0&original=0&twoPlayer=0&coins=0&epic=0&secret=Wmfd2893gb7";
                string Packet = SendPostRequest("http://boomlings.com/database/getGJLevels21.php", Params);
                string[] LevelPacket = Packet.Replace('~', ' ').Split('|', ':');
                return LevelPacket;
            }
        }
        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";
        }
    }
}
