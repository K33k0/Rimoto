using System;
using System.Collections.Generic;
using System.IO;
using Serilog;


namespace RimotoCore
{
    public class Run
    {


        public static List<Media> GatherLists()
        {

            var log = new LoggerConfiguration()
             .WriteTo.Console()
             .CreateLogger();
            Log.Logger = log;

            var allResults = new List<Media>();
            var SonarrResults = new List<Sonarr.Record>();
            var RadarrResults = new List<Radarr.Record>();

            try
            {
                SonarrResults = Sonarr.Connection.ReadUnscanned().Result;
                RadarrResults = Radarr.Connection.ReadUnscanned().Result;
            }
            catch (AggregateException ex)
            {
                log.Information(ex, "Failed to retrieve results");
            }

            foreach (var r in SonarrResults)
            {
                var result = new Media
                {
                    dateAdded = r.DateAdded,
                    ImportedPath = Plex.Scan.TransformPath(r.ImportedPath)
                };
                log.Information($"Added {r.ImportedPath} to scan queue.");
                allResults.Add(result);
            }
            foreach (var r in RadarrResults)
            {
                var result = new Media
                {
                    dateAdded = r.DateAdded,
                    ImportedPath = Plex.Scan.TransformPath(r.ImportedPath)
                };
                log.Information($"Added {r.ImportedPath} to scan queue.");

                allResults.Add(result);
            }

            allResults.RemoveAll(r => File.Exists(r.ImportedPath) == false);
            log.Debug($"{allResults.Count} Results compiled");
            return allResults;

        }


        public class Media
        {
            public DateTime dateAdded;
            public string ImportedPath;

        }

        public static void ScanMedia(string path)
        {
            var id = Plex.Scan.ParseLibraryId(path);

            Plex.Scan.scanFile(path, id);
        }
    }
}
