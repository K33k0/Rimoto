using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Sonarr
{
    public static class Connection
    {
        private  const string Base = "http://10.9.0.1:8989/api/v3";
        private const string ApiKey = "62e0eac137ed4339ae76f6596dfcf0e2";
        private static readonly HttpClient Client = new HttpClient();

        private static async Task<JArray> TransformRequest(string path)
        {
            var response = await Client.GetStringAsync(Base + path);
            JObject joResponse = JObject.Parse(response);
            JArray array = (JArray) joResponse["records"];
            return array;
        }
        
        public static async Task<List<Record>> ReadUnscanned()
        {
            string path = $"/history?apikey={ApiKey}&sortDir=asc&pageSize=500&eventType=3";
            var response = await TransformRequest(path);

            var SonarrResult = new List<Record>();
            // Parse the array and feed to caller
            foreach (var r in response)
            {
                SonarrResult.Add( new Record
                {
                    DateAdded = (DateTime) r["date"],
                    ImportedPath = (string) r["data"]["importedPath"],
                    Quality = (string) r["quality"]["quality"]["name"],
                });
            }

            return SonarrResult;

        }
    }

    public class Record
    {
        public DateTime DateAdded { get; set; }
        public string ImportedPath { get; set; }
        public string Quality { get; set; }
        
    }
}