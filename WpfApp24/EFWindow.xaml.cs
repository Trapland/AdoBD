using AdoBD.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    /// Interaction logic for EFWindow.xaml
    /// </summary>
    public partial class EFWindow : Window
    {
        public EfContext efContext { get; set; } = new();
        private ICollectionView depListView;   // інтерфейс для налагодження View з колекцією
        private static readonly Random random = new();
        public EFWindow()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            efContext.Departments.Load();
            depList.ItemsSource = efContext.Departments.Local.ToObservableCollection();
            // отримання посилання на depList, але як інтерфейс ICollectionView
            depListView = CollectionViewSource.GetDefaultView(depList.ItemsSource);
            depListView.Filter =   // Predicate<object>
                obj => (obj as Department)?.DeleteDt == null;  // TODO: замінити на DepartmentsDeletedFilter

            UpdateMonitor();
            UpdateDailyStatistics();
        }
        private void UpdateMonitor()
        {
            MonitorBlock.Text = "Departments: " + efContext.Departments.Count();
            MonitorBlock.Text += "\nProducts: " + efContext.Products.Count();
            MonitorBlock.Text += "\nManagers: " + efContext.Managers.Count();
            MonitorBlock.Text += "\nSales: " + efContext.Sales.Count();
        }
        private void UpdateDailyStatistics()
        {
            // загальна кількість чеків (записів Sales) за сьогодні
            var todaySales = efContext.Sales.Where(s => s.SaleDt.Date == DateTime.Today);
            SalesChecks.Content = todaySales.Count().ToString();  // .Count() - запускає запит
            var allSalesCnt = efContext.Sales;

            // загальна кількість проданих товарів (сума Sales.Quantity) за сьогодні
            SalesCnt.Content = allSalesCnt.Count().ToString();
            // фактичний час старту продажів сьогодні
            StartMoment.Content = todaySales.Min(s => s.SaleDt).ToString();
            // час останнього продажу
            FinishMoment.Content = todaySales.Max(s => s.SaleDt).ToString();
            // максимальна кількість товарів у одному чеку (за сьогодні)
            MaxCheckCnt.Content = todaySales.Max(s => s.Quantity).ToString();
            // "середній чек" за кількістю - середнє значення кількості проданих товарів на один чек
            AvgCheckCnt.Content = todaySales.Average(s => s.Quantity).ToString();
            // Повернення - чеки, що є видаленими (кількість чеків за сьогодні)
            DeletedCheckCnt.Content = todaySales.Count(s => s.DeleteDt != null).ToString();

            ///////////////////////////////////////////////////////////////////////////

            var queryMan = efContext.Managers
             .GroupJoin(
             todaySales,
             m => m.Id,
             s => s.ManagerId,
             (m, sales) => new
             {
                 Id = m.IdMainDep,
                 Name = m.Name,
                 Surname = m.Surname,
                 Cnt = sales.Count(),
                 Sum = sales.Join(efContext.Products, sale => sale.ProductId, product => product.Id, (sale, product) => sale.Quantity * product.Price).Sum()
             }
             ).OrderByDescending(g => g.Cnt);

            BestManager.Content = queryMan.First().Surname + " " + queryMan.First().Name;
            foreach (var item in queryMan)
            {
                BestManagersItems.Content += "\n" + item.Surname + " " + item.Name + "---" + item.Cnt;
                BestMoneyItems.Content += "\n" + item.Surname + " " + item.Name + "---" + item.Sum.ToString("0.00") + " UAH";
            }

            ///////////////////////////////////////////////
            
            var StatSales = efContext.Products
                  .GroupJoin(
                 todaySales,
                 p => p.Id,
                 s => s.ProductId,
                 (p, sales) => new
                 {
                     Name = p.Name,
                     Cnt = sales.Count(),
                     Prc = sales.Join(efContext.Products, sale => sale.ProductId, product => product.Id, (sale, product) => sale.Quantity * product.Price).Sum()
                 }
                 ).OrderByDescending(g => g.Cnt);

            foreach (var item in StatSales)
            {
                LogBlock.Text += $"{item.Name} -- {item.Cnt}  -- {item.Prc.ToString("0.00")} UAH \n";
            }

            BestProduct.Content = StatSales.First().Name;


            ///////////////////////////////////////////////////////


            var queryDeps = efContext.Departments.ToList()
             .GroupJoin(
             efContext.Managers.GroupJoin(
             todaySales,
             m => m.Id,
             s => s.ManagerId,
             (m, sales) => new
             {
                 IdDep = m.IdMainDep,
                 Name = m.Name,
                 Cnt = sales.Count(),
                 Sum = sales.Join(efContext.Products, sale => sale.ProductId, product => product.Id, (sale, product) => sale.Quantity * product.Price).Sum()
             }
             ).OrderByDescending(g => g.Cnt),
             d => d.Id,
             man => man.IdDep,
             (dep, managers) => new
             {
                 Name = dep.Name,
                 Cnt = managers.Sum(m => m.Cnt),
                 Sum = managers.Sum(m => m.Sum)
             }
             ).OrderByDescending(it => it.Cnt);

            DepartmentsStatList.ItemsSource = queryDeps;

            ////////////////////////////////////

        }
        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            CrudWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                // dialog.Department - інша сутність, треба змінити під EF
                efContext.Departments.Add(
                    new Department()
                    {
                        Name = dialog.Department.Name,
                        Id = dialog.Department.Id
                    });
                // !! додавання даних до контексту не додає їх до БД - планування додавання
                efContext.SaveChanges();  // внесення змін до БД

                MonitorBlock.Text += "\nDepartments: " + efContext.Departments.Count();
            }
        }
        private bool DepartmentsDeletedFilter(object item)
        {
            if (item is Department department)
            {
                return department.DeleteDt == null;
            }
            return false;
        }
        private void ShowAllDepsCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            depListView.Filter = DepartmentsDeletedFilter;
            ((GridView)depList.View).Columns[2].Width = 0;
        }
        private void ShowAllDepsCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            depListView.Filter = null; // скидаємо фільтр - показує усі дані

            ((GridView)depList.View)   // Властивості Visible для колонок ListView немає, тому
                .Columns[2]            // приховування/відображення - через встановлення Width
                .Width = Double.NaN;   // Double.NaN - автоматичне визначення
        }
        private void GenerateSalesButton_Click(object sender, RoutedEventArgs e)
        {
            // Випадковий менеджер з наявних
            // Manager manager =  // "-" отримання .ToList() передає всі дані, для BigData неприйнятно
            //    efContext.Managers.ToList()[random.Next(efContext.Managers.Count())];
            // Manager manager = // System.InvalidOperationException - LINQ-to-Entity перекладає запит на SQL. Не всі 
            //    efContext.Managers.ElementAt(random.Next(efContext.Managers.Count())); // можливості мови C# мають аналоги у SQL
            // DbSet приймає методи розширення LINQ, але не всі вони врешті спрацьовують, оскільки
            //    це LINQ-to-Entity (LINQ-to-SQL), що накладає певні обмеження

            double maxPrice = efContext.Products.Max(p => p.Price);
            int manCnt = efContext.Managers.Count();
            int proCnt = efContext.Products.Count();

            for (int i = 0; i < 100; i++)
            {
                Manager manager = efContext.Managers.Skip(random.Next(manCnt)).First();
                // Випадковий товар
                Product product = efContext.Products.Skip(random.Next(proCnt)).First();
                // Випадкова кількість, але чим дорожче товар, тим менша гранична кількість
                int quantity = random.Next(1,
                    (int)(20 * (1 - product.Price / maxPrice) + 2));

                efContext.Sales.Add(new()
                {
                    Id = Guid.NewGuid(),
                    ManagerId = manager.Id,
                    ProductId = product.Id,
                    Quantity = quantity,
                    SaleDt = DateTime.Today.AddSeconds(random.Next(0, 86400))  // Дата "сьогодні" але з випадковим часом
                });
            }
            efContext.SaveChanges();
            UpdateMonitor();
            UpdateDailyStatistics();
        }

    }
}
