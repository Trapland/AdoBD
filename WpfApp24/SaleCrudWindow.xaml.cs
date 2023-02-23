using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для SaleCrudWindow.xaml
    /// </summary>
    public partial class SaleCrudWindow : Window
    {
        public Entity.Sale Sale { get; set; }

        private ObservableCollection<Entity.Product> OwnerProducts;
        private ObservableCollection<Entity.Manager> OwnerManagers;

        public SaleCrudWindow(Entity.Sale Sale)
        {
            this.Sale = Sale;
            OwnerProducts = null!;
            OwnerManagers = null!;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is OrmWindow owner)
            {
                DataContext = Owner;
                OwnerProducts = owner.Products;
                OwnerManagers = owner.Managers;
            }
            else
            {
                MessageBox.Show("Owner is not OrmWindow");
                Close();
            }

            if (this.Sale is null)
            {
                Sale = new Entity.Sale();
                DeleteButton.IsEnabled = false;
            }
            else
            {
                ProductComboBox.SelectedItem =
                    OwnerProducts
                    .Where(d => d.Id == this.Sale.ProductId)
                    .First();
                ManagerComboBox.SelectedItem =
                    OwnerManagers
                    .Where(d => d.Id == this.Sale.ManagerId)
                    .First();

                DeleteButton.IsEnabled = true;
            }
            IdView.Text = this.Sale.Id.ToString();
            DateView.Text = this.Sale.SaleDt.ToString();
            QuantityView.Text = this.Sale.Quantity.ToString();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Sale is null) { return; }

            if (QuantityView.Text.Equals(String.Empty))
            {
                MessageBox.Show("Необхідно ввести кількість");
                QuantityView.Focus();
                return;
            }
            int cnt;
            try
            {
                cnt = Convert.ToInt32(QuantityView.Text);
            }
            catch
            {
                MessageBox.Show("Кількість не розпізнана (очікується число)");
                QuantityView.Focus();
                return;
            }

            if (ProductComboBox.SelectedItem is null)
            {
                MessageBox.Show("Необхідно вибрати товар");
                ProductComboBox.Focus();
                return;
            }
            if (ManagerComboBox.SelectedItem is null)
            {
                MessageBox.Show("Необхідно вибрати продавця");
                ManagerComboBox.Focus();
                return;
            }

            this.Sale.Quantity = cnt;

            if (ProductComboBox.SelectedItem is Entity.Product product)
                this.Sale.ProductId = product.Id;
            else
                MessageBox.Show("ProductComboBox.SelectedItem CAST Error");

            if (ManagerComboBox.SelectedItem is Entity.Manager manager)
                this.Sale.ManagerId = manager.Id;
            else
                MessageBox.Show("ManagerComboBox.SelectedItem CAST Error");

            this.DialogResult = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
