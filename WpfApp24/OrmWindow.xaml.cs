using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace AdoBD
{
    /// <summary>
    /// Логика взаимодействия для OrmWindow.xaml
    /// </summary>
    public partial class OrmWindow : Window
    {
        public ObservableCollection<Entity.Departments> Departments { get; set; }
        public ObservableCollection<Entity.Products> Products { get; set; }
        public ObservableCollection<Entity.Managers> Managers { get; set; }
        private SqlConnection _connection;
        public OrmWindow()
        {
            InitializeComponent();
            Departments = new();
            Products = new();
            Managers = new();
            DataContext = this;
            _connection = new(App.ConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new() { Connection = _connection };

                #region Load Departments
                cmd.CommandText = "SELECT D.Id, D.Name FROM Departments D";
                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Departments.Add(new Entity.Departments{ Id = reader.GetGuid(0), Name = reader.GetString(1)});
                }
                #endregion
                #region Load Departments
                cmd.Dispose();
                cmd.CommandText = "SELECT P.Id, P.Name, P.Price FROM Products P";
                reader.Close();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Products.Add(new Entity.Products { Id = reader.GetGuid(0), Name = reader.GetString(1),Price = reader.GetDouble(2) });
                }
                #endregion
                #region Load Departments
                cmd.Dispose();
                cmd.CommandText = "SELECT M.Id,M.Surname,M.Name,M.Secname FROM Managers M";
                reader.Close();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Managers.Add(new Entity.Managers { Id = reader.GetGuid(0), Surname = reader.GetString(1), Name = reader.GetString(2), Secname = reader.GetString(3) });
                }
                #endregion
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Window will be closed", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //работа с бд через ORM упрощена до работы с коллекцией
            //sender - item, который имеет ссылку на Entity.Departments в коллекции Departments
            if(sender is ListViewItem item)
            {
                if(item.Content is Entity.Departments department)
                {
                    MessageBox.Show(department.ToString());
                }
            }
        }
    }
}
