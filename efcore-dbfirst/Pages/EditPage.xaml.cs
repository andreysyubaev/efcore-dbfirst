using efcore_dbfirst.Models;
using efcore_dbfirst.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace efcore_dbfirst.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Brand> Brands { get; set; } = new();
        public ObservableCollection<Tag> Tags { get; set; } = new();
        public ObservableCollection<Tag> ProductTags { get; set; } = new();

        CategoryService categoryService = new();
        BrandService brandService = new();
        TagService tagService = new();
        ProductService service = new();

        public Product Product { get; set; }
        public Tag? SelectedTag { get; set; }
        public Tag? SelectedProductTag { get; set; }

        bool IsEdit = false;
        public EditPage(Product product)
        {
            InitializeComponent();

            Product = service.Products
                .First(p => p.Id == product.Id);

            DBService.Instance.Context.Entry(Product)
                .Collection(p => p.Tags)
                .Load();

            LoadCategories();
            LoadBrands();

            DataContext = this;
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            Tags.Clear();
            ProductTags.Clear();

            tagService.GetAll();

            foreach (var t in tagService.Tags)
                Tags.Add(t);

            if (Product.Tags != null)
            {
                foreach (var t in Product.Tags)
                {
                    ProductTags.Add(t);
                    Tags.Remove(t);
                }
            }
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

        private void Edit(object sender, RoutedEventArgs e)
        {
            service.Commit();
            MessageBox.Show("Изменения сохранены", "Готово",
                MessageBoxButton.OK, MessageBoxImage.Information);
            Back(sender, e);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (Product != null)
                if (MessageBox.Show(
                    "Вы действительно хотите удалить это?",
                    "Удалить группу?",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(Product);
                    Back(sender, e);
                }
                else
                    MessageBox.Show(
                        "Выберите для удаления",
                        "Выберите",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
