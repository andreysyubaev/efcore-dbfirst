using efcore_dbfirst.Models;
using efcore_dbfirst.Service;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        public ElectronicsStoreContext db = DBService.Instance.Context;
        public ICollectionView tagsView { get; set; }
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public ObservableCollection<Tag> ProductTags { get; set; } = new();

        CategoryService categoryService = new();
        BrandService brandService = new();
        TagService tagService = new();
        ProductService service = new();

        public Tag? SelectedTag { get; set; }
        public Tag? SelectedProductTag { get; set; }
        public Product Product { get; set; }
        bool IsEdit = false;
        public AddProductPage(Product? product = null)
        {
            InitializeComponent();

            Product = product ?? new Product();

            LoadCategories();
            LoadBrands();

            IsEdit = product != null;

            DataContext = this;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Добавление товара";
            }
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            Tags.Clear();
            tagService.GetAll();

            foreach (var c in tagService.Tags)
                Tags.Add(c);
        }

        private void LoadCategories()
        {
            Categories.Clear();
            categoryService.GetAll();

            foreach (var c in categoryService.Categories)
                Categories.Add(c);
        }

        private void LoadBrands()
        {
            Brands.Clear();
            brandService.GetAll();

            foreach (var b in brandService.Brands)
                Brands.Add(b);
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(Description.Text) || string.IsNullOrEmpty(Price.Text)
                || string.IsNullOrEmpty(Stock.Text) || string.IsNullOrEmpty(Rating.Text) || string.IsNullOrEmpty(CreatedAt.Text))
            {
                MessageBox.Show("Заполните все поля", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (Product.CategoryId == 0)
            {
                MessageBox.Show(
                    "Выберите категорию",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else if (Product.BrandId == 0)
            {
                MessageBox.Show(
                    "Выберите бренд",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else
            {
                if (IsEdit)
                    service.Commit();
                else
                    service.Add(Product);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddTag(object sender, RoutedEventArgs e)
        {
            if (SelectedTag == null)
            {
                MessageBox.Show("Выберите тег", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Product.Tags ??= new List<Tag>();

            Product.Tags.Add(SelectedTag);
            ProductTags.Add(SelectedTag);
            Tags.Remove(SelectedTag);

            SelectedTag = null;
        }

        private void RemoveTag(object sender, RoutedEventArgs e)
        {
            if (SelectedProductTag == null)
            {
                MessageBox.Show("Выберите тег", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Product.Tags?.Remove(SelectedProductTag);
            Tags.Add(SelectedProductTag);
            ProductTags.Remove(SelectedProductTag);

            SelectedProductTag = null;
        }
    }
}
