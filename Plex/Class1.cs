using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Net.Mime;
using Serilog;

namespace Plex
{
    public static class Database
    {
        public const string DatabasePath = @"URI=file:C:/Users/keewy/AppData/Local/Plex Media Server/Plug-in Support/Databases/com.plexapp.plugins.library.db";

        public static bool DoesExists(string path)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;
            
            using var con = new SQLiteConnection(DatabasePath);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "Select count(file) from media_parts WHERE file = @path";
            cmd.Parameters.AddWithValue("@path", path);
            cmd.Prepare();
            var fileCount = (Int64)cmd.ExecuteScalar();
            con.Close();
            return fileCount != 0;
        }

        public static DateTime LastUpdated(string path)
        {
            var lastUpdate = new DateTime(2000, 1, 1);
            using var con = new SQLiteConnection(DatabasePath);
            con.Open();
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "Select updated_at from media_parts WHERE file = @path";
            cmd.Parameters.AddWithValue("@path", path);
            cmd.Prepare();
            try
            {
                lastUpdate = (DateTime)cmd.ExecuteScalar();
            } catch {}


            con.Close();
            return lastUpdate;
        }
    }
     
    
    public static class Scan
    {

        const string ScannerPath = "C:/Program Files (x86)/Plex/Plex Media Server/Plex Media Scanner.exe";
        private const string basePath = "C:/Media";
        private const string remoteMountPath = "gcache";


        public static string TransformPath(string mediaPath)
        {
            var strippedPath = Regex.Split(mediaPath, $".*?{remoteMountPath}")[1];
            return $"{basePath}{strippedPath}";
        }

        public static int ParseLibraryId(string mediaPath)
        {
            if (mediaPath.Contains("Movies")){return 2;}
            if (mediaPath.Contains("Anime")){return 3;}
            if (mediaPath.Contains("Adult")){return 11;}
            if (mediaPath.Contains("Kids")){return 12;}
            if (mediaPath.Contains("Family")){return 13;}

            throw new NotSupportedException("Library could not be parsed.");
        }

        public static bool DoesPathExist(string mediaPath) =>  File.Exists(mediaPath);

        public static void scanFile(string mediaPath, int libraryId)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;

            if (Database.DoesExists(mediaPath))
            {
                Log.Error($"{mediaPath} does not exist.");
                return;
            }

            if (!File.Exists(ScannerPath))
            {
                Log.Error($"{ScannerPath} does not exist");
            }

                using (Process proc = new Process())
            {
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = $@"{(char)34}{ScannerPath}{(char)34}";
                proc.StartInfo.Arguments =
                    $@"-c {libraryId} -s -r -d {(char)34}{Path.GetDirectoryName(mediaPath)}{(char)34}";
                proc.Start();
                Console.WriteLine();
                Console.Write($"{proc.Id}: {mediaPath} : Scanning");
                do
                {
                    if (!proc.HasExited)
                    {
                        proc.Refresh();
                        if (proc.Responding)
                        {
                            Console.Write($"\r{DateTime.Now}: {proc.Id}: {mediaPath} :  Waiting");
                        }
                        else
                        {
                            Console.Write($"{DateTime.Now}: {proc.Id}: {mediaPath} : No Response");
                        }
                    }
                } while (!proc.WaitForExit(1000));
                Console.Write($"\r{DateTime.Now}: {proc.Id}: {mediaPath} : Complete > {proc.ExitCode}");
                Console.WriteLine();
            }
            
        }

    }
}