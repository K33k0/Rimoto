using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Windows.UI.Notifications;
using Microsoft.VisualBasic;
using Plex;
using Radarr;
using Serilog;
using Record = Sonarr.Record;

namespace RimotoCli
{

    class Program


    {
        private static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;
            
            Log.Information(("The Global logger has been configured"));
            
            var maxAge = 30;
            Log.Information($"Accepting files that are less than {maxAge} days old");
            
            var SonarrResults = new List<Sonarr.Record>();
            var RadarrResults = new List<Radarr.Record>();
            
            while (true)
            {
                var Media = RimotoCore.Run.GatherLists();
                foreach (var r in Media)
                {
                    RimotoCore.Run.ScanMedia(r.ImportedPath);
                }
                
                Log.Information($"Finished loop. Waiting 10 minutes.");
                Console.WriteLine();
                Console.WriteLine();
                Thread.Sleep(60000 * 10);
            }
        }
        

        
    }
}