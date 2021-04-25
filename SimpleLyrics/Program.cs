using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SimpleLyrics
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Start();

            while (App.IsActive)
            {
                Song CurrentSong = PrepareSong();
                JObject JSONLyrics = GetJSONLyrics(CurrentSong);

                WriteLyrics(JSONLyrics);
                ControlMenuState();
            }
        }
        static JObject GetJSONLyrics(Song CurrentSong)
        {
            string json = GET(CurrentSong);
            JObject jObject = ParseJson(json);

            return jObject;
        }
        static JObject ParseJson(string json)
        {
            json = ProcessStringForJSON(json);
            JObject jObject = JObject.Parse(json);

            return jObject;
        }
        static string GET(Song song)
        {
            WebResponse webResponse = GetWebResponse(song.Compositor + "/" + song.Name);
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
        static Song PrepareSong()
        {
            Song song = CreateSong();
            ProcessSongDataForRequest(song);

            return song;
        }
        static void ProcessSongDataForRequest(Song song)
        {
            song.Compositor = ProcessStringForRequest(song.Compositor);
            song.Name = ProcessStringForRequest(song.Name);
        }
        static Song CreateSong()
        {
            string Compositor = ReadStringWithMessage("Введите название группы");
            string SongName = ReadStringWithMessage("Введите название композиции");

            Song song = new Song(Compositor, SongName);

            return song;
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
        static void ControlMenuState()
        {
            char key = AskPermisionForContinue();
            ChangeAppState(key);
        }
        static char AskPermisionForContinue()
        {
            Console.WriteLine("Желаете продолжить (Y/N): ");
            char key = GetPressedKeyLowerCase();

            return key;
        }
        static void ChangeAppState(char key)
        {
            if (key != 'y') App.Stop();
            else App.Start();
        }
        static char GetPressedKeyLowerCase()
        {
            char ch = Console.ReadKey().KeyChar;
            Console.Clear();
            ch = ch.ToString().ToLower()[0];

            return ch;
        }
    }
}