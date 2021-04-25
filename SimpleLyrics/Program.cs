using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SimpleLyrics
{
    class Program
    {
        static string MusicGroup;
        static string SongName;

        static void Main(string[] args)
        {
            bool IsRunning = true;

            while(IsRunning)
            {
                Menu();

                string json = GET(MusicGroup, SongName);
                JObject jObject = ParseJson(json);

                WriteLyrics(jObject);

                Console.WriteLine("Желаете продолжить (Y/N): ");
                char key = GetPressedKeyLowerCase();
                IsRunning = SelectAppState(key);
            }
        }
        static void Menu()
        {
            Console.Clear();

            MusicGroup = ReadStringWithMessage("Введите название группы");
            SongName = ReadStringWithMessage("Введите название композиции");

            MusicGroup = ProcessStringForRequest(MusicGroup);
            SongName = ProcessStringForRequest(SongName);
        }
        static JObject ParseJson(string json)
        {
            json = ProcessStringForJSON(json);
            JObject jObject = JObject.Parse(json);

            return jObject;
        }
        static string GET(string MusicGroup, string SongName)
        {
            WebResponse webResponse = GetWebResponse(MusicGroup + "/" + SongName);
            string output = ReadResponse(webResponse);

            return output;
        }
        static string ReadResponse(WebResponse webResponse)
        {
            if (webResponse == null) return "{lyrics: \"Not found\"}";

            using Stream stream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            string output = reader.ReadToEnd();

            return output;
        }
        static WebResponse GetWebResponse(string Data)
        {
            WebRequest webRequest = WebRequest.Create("https://api.lyrics.ovh/v1/" + Data);
            WebResponse webResponse = null;

            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch {}

            return webResponse;
        }
        static void WriteLyrics(JObject jObject)
        {
            Console.Clear();
            Console.WriteLine(jObject["lyrics"]);
        }
        static string ProcessStringForJSON(string str)
        {
            if (str.Contains("\\n\\n"))
                str = str.Replace("\\n\\n", "\\r\\n");

            return str;
        }
        static string ProcessStringForRequest(string str)
        {
            if(str.Contains(" "))
                str = str.Replace(" ", "%20");

            return str;
        }
        static string ReadStringWithMessage(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
        static bool SelectAppState(char key)
        {
            if (key == 'y') return true;
            else return false;
        }
        static char GetPressedKeyLowerCase()
        {
            char ch = Console.ReadKey().KeyChar;
            ch = ch.ToString().ToLower()[0];

            return ch;
        }
    }
}
