using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Backend
{
    public class RemoteDb
    {
        private MySqlConnection _connection;
        public string Errors;
        
        private void OpenConnection(string connectionString)
        {
            try
            {
                // Connect to the database. Assign it to a class variable.
                 _connection = new MySqlConnection(connectionString);
            }
            catch (Exception e)
            {
                // Store error message in class var. throw the exception again.
                // Gives chance for the app to catch
                Errors = e.Message;
                throw;
            }
        }
        
        
    }

    public class PlexDb
    {
        
    } 
}