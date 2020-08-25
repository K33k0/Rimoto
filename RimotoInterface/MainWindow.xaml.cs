using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RimotoInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CurrentStatus =
            DependencyProperty.Register("State", typeof(List), typeof(MainWindow));

        public string State
        {
            get => this.GetValue(CurrentStatus) as string;
            set => this.SetValue(CurrentStatus, value);
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
