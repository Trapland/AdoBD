using AdoBD.Entity;
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
        public ObservableCollection<Entity.Department> Departments { get; set; }
        public ObservableCollection<Entity.Product> Products { get; set; }
        public ObservableCollection<Entity.Manager> Managers { get; set; }
        private SqlConnection _connection;
        public CrudWindow _dialogDepartment;
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
                while (reader.Read())
                {
                    Departments.Add(new Entity.Department { Id = reader.GetGuid(0), Name = reader.GetString(1) });
                }
                #endregion
                #region Load Products
                cmd.Dispose();
                cmd.CommandText = "SELECT P.Id, P.Name, P.Price FROM Products P";
                reader.Close();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Products.Add(new Entity.Product { Id = reader.GetGuid(0), Name = reader.GetString(1), Price = reader.GetDouble(2) });
                }
                #endregion
                #region Load Managers
                cmd.Dispose();
                cmd.CommandText = "SELECT M.Id, M.Surname, M.Name, M.Secname, M.Id_main_dep, M.Id_sec_dep, M.Id_chief  FROM Managers M";
                reader.Close();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Managers.Add(new Entity.Manager
                    {
                        Id = reader.GetGuid(0),
                        Surname = reader.GetString(1),
                        Name = reader.GetString(2),
                        Secname = reader.GetString(3),
                        Id_main_dep = reader.GetGuid(4),
                        Id_sec_dep = reader.GetValue(5) == DBNull.Value ? null : reader.GetGuid(5),
                        Id_chief = reader.IsDBNull(6) ? null : reader.GetGuid(6),

                    });
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
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Department department)
                {
                    _dialogDepartment = new();
                    _dialogDepartment.Department = department;
                    SqlCommand cmd = new() { Connection = _connection };
                    if (_dialogDepartment.ShowDialog() == true)
                    {
                        if (_dialogDepartment.Department is null) //Delete
                        {
                            try
                            {

                                cmd.CommandText = $"Update Departments SET Name = 'Empty' WHERE Id = '{department.Id}'";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                MessageBox.Show("Deleted");
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Delete error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else //Update
                        {
                            try
                            {

                                cmd.CommandText = $"Update Departments SET Name = '{department.Name}' WHERE Id = '{department.Id}'";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                MessageBox.Show(department.ToString());
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void Products_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Product product)
                {
                    MessageBox.Show(product.ToString());
                }
            }

        }

        private void Managers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Manager manager)
                {
                    MessageBox.Show(manager.ToString());
                }
            }
        }

        private void Create_Dep_Click(object sender, RoutedEventArgs e)
        {
            Entity.Department dep = new();
            dep.Id = generateID();
            dep.Name = "";
            _dialogDepartment = new();
            _dialogDepartment.Department = dep;
            SqlCommand cmd = new() { Connection = _connection };
            if (_dialogDepartment.ShowDialog() == true)
            {
                try
                {
                    for (int i = 0; i < Departments.Count; i++)
                    {
                        if (dep.Name == Departments[i].Name)
                        {
                            MessageBox.Show("this name already exists");
                            return;
                        }
                    }
                    cmd.CommandText = $"INSERT INTO Departments (Id, Name) VALUES('{dep.Id}', '{dep.Name}')";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    MessageBox.Show(dep.ToString());
                    Departments.Add(dep);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public Guid generateID()
        {
            return Guid.NewGuid();
        }
    }
}
