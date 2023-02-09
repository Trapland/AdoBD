using AdoBD.Entity;
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
using System.Windows.Shapes;

namespace AdoBD
{
    /// <summary>
    /// Логика взаимодействия для CrudWindow.xaml
    /// </summary>
    public partial class CrudWindow : Window
    {
        public Entity.Department Department { get; set; }
        public CrudWindow()
        {
            InitializeComponent();
            Department = null!;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Department is null)
            {
                DeleteButton.IsEnabled= false;
            }
            else
            {
                Id.Text = Department.Id.ToString();
                Name.Text = Department.Name;
                DeleteButton.IsEnabled = true;

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Department.Name = Name.Text;
            this.DialogResult = true; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Department = null!;
            this.DialogResult = true; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; //то, что вернёт ShowDialog
            //this.Close();
        }
    }
}
