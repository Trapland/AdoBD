using AdoBD.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Логика взаимодействия для ManagerCrudWindow.xaml
    /// </summary>
    public partial class ManagerCrudWindow : Window
    {
        public Entity.Manager? Manager { get; set; }
        private ObservableCollection<Entity.Department> OwnerDepartments { get; set; }
        public ManagerCrudWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Owner is OrmWindow owner)
            {
                DataContext = Owner;
                OwnerDepartments = owner.Departments;
            }
            else
            {
                MessageBox.Show("Owner is not OrmWindow");
                Close();
            }
            if (this.Manager is null)
            {
                Manager= new Entity.Manager();

            }
            else
            {
                Surname.Text = this.Manager.Surname;
                Name.Text = this.Manager.Name;
                Secname.Text = this.Manager.Secname;
                MainDep.SelectedItem = OwnerDepartments.Where(d => d.Id == this.Manager.Id_main_dep).FirstOrDefault();
                SecDep.SelectedItem = OwnerDepartments.Where(d => d.Id == this.Manager.Id_sec_dep).FirstOrDefault();
                Chief.SelectedItem = (Owner as OrmWindow)?.Managers.Where(m => m.Id == this.Manager.Id_chief).FirstOrDefault();
            }
            Id.Text = this.Manager.Id.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
            {
                MessageBox.Show("Name is empty, can`t be saved");
                return;
            }
            else if (Name.Text == Manager.Name)
            {
                this.DialogResult = false;
                return;
            }
            if (Surname.Text == "")
            {
                MessageBox.Show("Name is empty, can`t be saved");
                return;
            }
            else if (Surname.Text == Manager.Surname)
            {
                this.DialogResult = false;
                return;
            }
            if (Secname.Text == "")
            {
                MessageBox.Show("Name is empty, can`t be saved");
                return;
            }
            else if (Secname.Text == Manager.Secname)
            {
                this.DialogResult = false;
                return;
            }
            Manager.Name = Name.Text;
            Manager.Surname = Surname.Text;
            Manager.Secname = Secname.Text;
            if(MainDep.SelectedItem is Entity.Department dep)
            {
                Manager.Id_main_dep= dep.Id;
            }
            else
            {
                MessageBox.Show("MainDepComboBox.SelectedItem Cast Error");
            }
            if (SecDep.SelectedItem is Entity.Department depart)
            {
                Manager.Id_sec_dep = depart.Id;
            }
            else
            {
                MessageBox.Show("SecDepComboBox.SelectedItem Cast Error");
            }
            if (Chief.SelectedItem is Entity.Manager manag)
            {
                Manager.Id_chief = manag.Id;
            }
            else
            {
                MessageBox.Show("ChiefComboBox.SelectedItem Cast Error");
            }
            this.DialogResult = true; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete?", "Delete message", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Manager = null!;
                this.DialogResult = true; //то, что вернёт ShowDialog
            }
            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void ClearSecDep_Click(object sender, RoutedEventArgs e)
        {
            SecDep.SelectedItem = null;

        }

        private void ClearChief_Click(object sender, RoutedEventArgs e)
        {
            Chief.SelectedIndex = -1;
        }
    }
}
