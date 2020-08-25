using System;
using System.Collections.Generic;
using Serilog;

namespace RimotoInteractive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pulling media");
            var SonarrResults = new List<Sonarr.Record>();
            var RadarrResults = new List<Radarr.Record>();
            SonarrResults = Sonarr.Connection.ReadUnscanned().Result;
            RadarrResults = Radarr.Connection.ReadUnscanned().Result;
            
            Console.WriteLine("Total days history to pull: ");
            var totalDaysHistoryInput = Console.ReadLine();
            var totalDaysHistory =  Convert.ToInt32(totalDaysHistoryInput);
            Console.WriteLine("Removing old results");
            SonarrResults.RemoveAll(r => (r.DateAdded - DateTime.Now).Days > totalDaysHistory);
            RadarrResults.RemoveAll(r => (r.DateAdded - DateTime.Now).Days > totalDaysHistory);
            Console.WriteLine($"Remaining results: {SonarrResults.Count + RadarrResults.Count}");
            Console.WriteLine("Listing Sonarr results.");
            for (int i = 0; i < SonarrResults.Count; i++)
            {
                var row = SonarrResults[i]; 
                Console.WriteLine($"[{i}]    {row.DateAdded}: {row.ImportedPath}");
            }
        }
    }
}