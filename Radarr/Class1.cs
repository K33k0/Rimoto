using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Radarr
{
    public static class Connection
    {
        private const string Base = "http://10.9.0.1:7878/api";
        private const string ApiKey = "95011e60db5f43e1834e4835dcadda90";
        private static readonly HttpClient _client = new HttpClient();

        public async static Task<JArray> TransformRequest(string path)
        {
            var response = await _client.GetStringAsync(Base + path);
            JObject joResponse = JObject.Parse(response);
            JArray array = (JArray) joResponse["records"];
            return array;
        }
        
        public async static Task<List<Record>> ReadUnscanned()
        {
            string path = $"/history?page=1&apikey={ApiKey}&pageSize=500&sortKey=date&sortDir=desc&filterKey=eventType&filterValue=3&filterType=equal";
            var response = await TransformRequest(path);

            var RadarrResult = new List<Record>();
            // Parse the array and feed to caller
            foreach (var r in response)
            {
                RadarrResult.Add(new Record
                {
                    DateAdded = (DateTime) r["date"],
                    ImportedPath = (string) r["data"]["importedPath"],
                    Quality = (string) r["quality"]["quality"]["name"],
                    Title = (string) r["movie"]["title"],
                    Year = (int) r["movie"]["year"],
                });
            }

            return RadarrResult;
        }
    }

    public class Record
    {
        public DateTime DateAdded { get; set; }
        public string ImportedPath { get; set; }
        public string Quality { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        
        
    }
}