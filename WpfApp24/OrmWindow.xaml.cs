using AdoBD.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
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
        public ProductCrudWindow _dialogProduct;
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
                cmd.CommandText = "SELECT D.* FROM Departments D";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Departments.Add(new Entity.Department(reader));
                }
                #endregion
                #region Load Products
                cmd.Dispose();
                cmd.CommandText = "SELECT P.* FROM Products P WHERE DeleteDt IS NULL";
                reader.Close();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Products.Add(new Entity.Product(reader));
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

                                cmd.CommandText = $"Update Departments SET Name = N'{department.Name}' WHERE Id = '{department.Id}'";
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
                    cmd.CommandText = $"INSERT INTO Departments (Id, Name) VALUES('{dep.Id}', N'{dep.Name}')";
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

        private void Products_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //работа с бд через ORM упрощена до работы с коллекцией
            //sender - item, который имеет ссылку на Entity.Departments в коллекции Departments
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Product product)
                {
                    _dialogProduct = new();
                    _dialogProduct.Product = product;
                    SqlCommand cmd = new() { Connection = _connection };
                    if (_dialogProduct.ShowDialog() == true)
                    {
                        if (_dialogProduct.Product is null) //Delete
                        {
                            try
                            {

                                cmd.CommandText = $"Update Products SET Name = 'Empty', Price = 0 WHERE Id = '{product.Id}'";
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
                                cmd.CommandText = $"Update Products SET Name = N'{product.Name}',Price = {product.Price.ToString(CultureInfo.InvariantCulture)} WHERE Id = '{product.Id}'";
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                                MessageBox.Show(product.ToString());
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

        private void Create_Prod_Click(object sender, RoutedEventArgs e)
        {
            Entity.Product prod = new();
            prod.Id = generateID();
            prod.Name = "";
            _dialogProduct = new();
            _dialogProduct.Product = prod;
            //SqlCommand cmd = new() { Connection = _connection };
            if (_dialogProduct.ShowDialog() == true)
            {
                String sql = "INSERT INTO Products(Id,Name,Price) VALUES (@id, @name, @price)";
                using SqlCommand cmd = new(sql, _connection);
                cmd.Parameters.AddWithValue("@id", prod.Id);
                cmd.Parameters.AddWithValue("@name", prod.Name);
                cmd.Parameters.AddWithValue("@price", prod.Price);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //не рекомендуется использовать
                //try
                //{
                //    for (int i = 0; i < Products.Count; i++)
                //    {
                //        if (prod.Name == Products[i].Name)
                //        {
                //            MessageBox.Show("this name already exists");
                //            return;
                //        }
                //    }
                //    cmd.CommandText = $"INSERT INTO Products (Id, Name, Price) VALUES('{prod.Id}', N'{prod.Name}', {prod.Price.ToString(CultureInfo.InvariantCulture)})";
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();
                //    MessageBox.Show(prod.ToString());
                //    Products.Add(prod);
                //}
                //catch (SqlException ex)
                //{
                //    MessageBox.Show(ex.Message, "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                //}


            }
        }

        private void Managers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Manager manager)
                {
                    ManagerCrudWindow dialog = new ManagerCrudWindow() { Owner = this};
                    dialog.Manager = manager;
                    dialog.ShowDialog();
                }
            }
        }

        public Guid generateID()
        {
            return Guid.NewGuid();
        }
    }
}
