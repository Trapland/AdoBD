using AdoBD.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AdoBD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly String ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\boyar\source\repos\WpfApp24\WpfApp24\Database1.mdf;Integrated Security=True";
        internal static readonly Logger Logger = new("Log.txt");
    }
}
