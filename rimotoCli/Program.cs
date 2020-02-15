using System;

namespace rimotoCli
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            foreach (var r in RemoteDb.Connection.FetchUnscannedMedia())
            {
                Console.WriteLine($"{r.Id.ToString()}: {r.DateAdded} - {r.ServerPath}");
            }
        }
    }
}