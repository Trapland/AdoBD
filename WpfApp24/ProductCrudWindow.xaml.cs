using AdoBD.Entity;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ProductCrudWindow.xaml
    /// </summary>
    /// 
    public partial class ProductCrudWindow : Window
    {
        public Entity.Product Product { get; set; }
        public ProductCrudWindow()  
        {
            InitializeComponent();
            Product = null!;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Product is null)
            {
                DeleteButton.IsEnabled = false;
            }
            else
            {
                Id.Text = Product.Id.ToString();
                Name.Text = Product.Name;
                Price.Text = Product.Price.ToString();
                DeleteButton.IsEnabled = true;

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
            {
                MessageBox.Show("Name is empty, can`t be saved");
                return;
            }
            else if (Name.Text == Product.Name)
            {
                this.DialogResult = false;
                return;
            }
            Product.Name = Name.Text;
            Product.Price = Convert.ToDouble(Price.Text,CultureInfo.InvariantCulture);
            this.DialogResult = true; //то, что вернёт ShowDialog
            //this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete?", "Delete message", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Product = null!;
                this.DialogResult = true; //то, что вернёт ShowDialog
            }
            //this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; //то, что вернёт ShowDialog
            //this.Close();
        }
    }
}
