using AdoBD.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
    /// Логика взаимодействия для DalWindow.xaml
    /// </summary>
    public partial class DalWindow : Window
    {
        private readonly DAL.DataContext dataContext;

        public ObservableCollection<Entity.Department> DepartmentList { get; set; }

        public ObservableCollection<Entity.Manager> ManagerList { get; set; }

        CrudWindow _dialogDepartment;
        public DalWindow()
        {
            InitializeComponent();
            dataContext= new();
            DepartmentList= new(dataContext.Departments.GetAll());
            ManagerList = new(dataContext.Managers.GetAll());
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(dataContext.Departments.GetAll().Count.ToString());
        }

        private void DepartmentItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //работа с бд через ORM упрощена до работы с коллекцией
            //sender - item, который имеет ссылку на Entity.Departments в коллекции Departments
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Department department)
                {
                    MessageBox.Show(department.ToString());
                }
            }
        }

        private void Create_Dep_Click(object sender, RoutedEventArgs e)
        {
            Entity.Department dep = new();
            _dialogDepartment = new();
            _dialogDepartment.Department = dep;
            if (_dialogDepartment.ShowDialog() == true)
            {
                try
                {
                    for (int i = 0; i < DepartmentList.Count; i++)
                    {
                        if (dep.Name == DepartmentList[i].Name)
                        {
                            MessageBox.Show("this name already exists");
                            return;
                        }
                    }
                    MessageBox.Show(dep.ToString());
                    if(dataContext.Departments.Add(dep))
                    {
                        DepartmentList.Add(dep);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Create_Man_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManagersItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Entity.Manager manager)
                {
                    MessageBox.Show(manager.ToString());
                }
            }
        }
    }
}
