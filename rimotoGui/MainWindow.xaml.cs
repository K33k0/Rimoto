using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using RemoteDb;
namespace rimotoGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            List<Row> Rows = new List<Row>();
            foreach (var r in RemoteDb.Connection.FetchUnscannedMedia())
            {
                Rows.Add(new Row(){ Id = r.Id, VpsPath = r.VpsPath, PlexLibraryId = r.PlexLibraryId, DateAdded = r.DateAdded, ServerPath = r.ServerPath});
            }

            RowList.ItemsSource = Rows;
        }
    }

    public class Row
    {
        public int Id { get; set; }
        public string VpsPath { get; set; }
        public int PlexLibraryId { get; set; }
        public DateTime DateAdded { get; set; }
        public string ServerPath { get; set; }    
    }
}