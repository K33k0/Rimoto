using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace RemoteDb
{
    public class Connection
    {
        private const string RemoteDbConnectionString = @"server=37.187.113.74;uid=k33k00;pwd=jNzZL400wdJy;database=Rimoto";
        public static List<DbRow> FetchUnscannedMedia()
        {
            List<DbRow> allRows = new List<DbRow>();
            using (MySqlConnection conn = new MySqlConnection(RemoteDbConnectionString))
            {
                using (MySqlCommand comm = new MySqlCommand(@"SELECT * FROM Media WHERE date_scanned is null"  , conn))
                {
                    conn.Open();
                    MySqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        DbRow row = new DbRow();
                        int id = reader.GetInt32(0);
                        string vpsPath = reader.GetString(1);
                        int plexLibraryId = reader.GetInt32(2);
                        DateTime dateAdded = reader.GetDateTime(3);
                        string serverPath = reader.GetString(5);
                        row.GetValues(id, vpsPath, plexLibraryId, dateAdded, serverPath);
                        allRows.Add(row);
                    }

                    return allRows;
                }
            }
        }

        public static int DropRow(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(RemoteDbConnectionString))
            {
                using (MySqlCommand comm = new MySqlCommand(@"DELETE FROM Media WHERE id = @id",conn))
                {
                    SqlParameter idParam = new SqlParameter("id", SqlDbType.Int);
                    idParam.Value = id;
                    comm.Parameters.Add(idParam);
                    int total = comm.ExecuteNonQuery();
                    return total;
                }
            }
        }

        public struct DbRow
        {
            public int Id;
            public string VpsPath;
            public int PlexLibraryId;
            public DateTime DateAdded;
            public string ServerPath;
            
            public void GetValues(int dbId, string dbVpsPath, int dbPlexLibraryId, DateTime dbDateAdded,
                string dbServerPath)
            {
                Id = dbId;
                VpsPath = dbVpsPath;
                PlexLibraryId = dbPlexLibraryId;
                DateAdded = dbDateAdded;
                ServerPath = dbServerPath;
            }
        }

    }
}