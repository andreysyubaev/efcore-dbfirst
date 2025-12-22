using efcore_dbfirst.Models;
using efcore_dbfirst.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace efcore_dbfirst.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ElectronicsStoreContext db = DBService.Instance.Context;
        public ObservableCollection<Product> products { get; set; } = new();
        public ICollectionView productsView { get; set; }

        public Product? product { get; set; } = null;

        public string searchQuery { get; set; } = null!;
        public string filterPriceFrom { get; set; } = null!;
        public string filterPriceTo { get; set; } = null!;
        bool _manager = false;
        public MainPage(bool manager)
        {
            productsView = CollectionViewSource.GetDefaultView(products);
            productsView.Filter = FilterForms;
            InitializeComponent();
            if (manager)
            {
                managerAddButton.Visibility = Visibility.Visible;
                _manager = true;
            }
            else
            {
                managerAddButton.Visibility = Visibility.Collapsed;
            }
            DataContext = this;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Товары электроники";
            }
        }

        public bool FilterForms(object obj)
        {
            if (obj is not Product)
                return false;

            var product = (Product)obj;

            if (searchQuery != null && !product.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;

            if (!filterPriceFrom.IsNullOrEmpty() && Convert.ToDecimal(filterPriceFrom) > product.Price)
                return false;

            if (!filterPriceTo.IsNullOrEmpty() && Convert.ToDecimal(filterPriceTo) < product.Price)
                return false;

            return true;
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            products.Clear();

            var loadedProducts = db.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Tags)
                .ToList();
            foreach (var product in loadedProducts)
            {
                //Debug.WriteLine($"Продукт: {product.Name}");
                //Debug.WriteLine($"Категория: {product.Category?.Name ?? "null"}");
                //Debug.WriteLine($"Теги: {product.Tags?.Count ?? 0}");

                products.Add(product);
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            // 1. Очистка UI
            searchText.Text = string.Empty;
            priceFromText.Text = string.Empty;
            priceToText.Text = string.Empty;

            PriceComboBox.SelectedIndex = -1;
            StockComboBox.SelectedIndex = -1;
            SortComboBox.SelectedIndex = -1;

            // 2. Сброс значений фильтра
            searchQuery = null;
            filterPriceFrom = null;
            filterPriceTo = null;

            // 3. Очистка сортировки
            productsView.SortDescriptions.Clear();

            // 4. Обновление отображения
            productsView.Refresh();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage());
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPage(product));
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchQuery = searchText.Text;
            productsView.Refresh();
        }

        private void FilterPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterPriceFrom = priceFromText.Text;
            filterPriceTo = priceToText.Text;
            productsView.Refresh();
        }

        private void Price_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null)
                return;

            var cb = sender as ComboBox;
            if (cb?.SelectedItem is not ComboBoxItem selected)
                return;

            productsView.SortDescriptions.Clear();

            switch (selected.Tag?.ToString())
            {
                case "ascendingPrice":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Price", ListSortDirection.Ascending));
                    break;

                case "descendingPrice":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Price", ListSortDirection.Descending));
                    break;
            }

            productsView.Refresh();
        }


        private void Stock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null)
                return;

            var cb = sender as ComboBox;
            if (cb?.SelectedItem is not ComboBoxItem selected)
                return;

            productsView.SortDescriptions.Clear();

            switch (selected.Tag?.ToString())
            {
                case "ascendingStock":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Stock", ListSortDirection.Ascending));
                    break;

                case "descendingStock":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Stock", ListSortDirection.Descending));
                    break;
            }

            productsView.Refresh();
        }


        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null)
                return;

            var cb = sender as ComboBox;
            if (cb?.SelectedItem is not ComboBoxItem selected)
                return;

            productsView.SortDescriptions.Clear();

            switch (selected.Tag?.ToString())
            {
                case "categoriesAZ":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Category.Name", ListSortDirection.Ascending));
                    break;

                case "categoriesZA":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Category.Name", ListSortDirection.Descending));
                    break;

                case "brandsAZ":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Brand.Name", ListSortDirection.Ascending));
                    break;

                case "brandsZA":
                    productsView.SortDescriptions.Add(
                        new SortDescription("Brand.Name", ListSortDirection.Descending));
                    break;
            }

            productsView.Refresh();
        }


        private void ProductsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_manager)
            {
                if (product != null)
                    NavigationService.Navigate(new EditPage(product));
                else
                    MessageBox.Show("Выберите товар");
            }
        }
    }
}
