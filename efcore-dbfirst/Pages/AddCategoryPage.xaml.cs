using efcore_dbfirst.Models;
using efcore_dbfirst.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace efcore_dbfirst.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryPage.xaml
    /// </summary>
    public partial class AddCategoryPage : Page
    {
        Category _category = new();
        CategoryService service = new();
        bool IsEdit = false;
        public AddCategoryPage(Category? category = null)
        {
            InitializeComponent();

            if (category != null)
            {
                _category = category;
                IsEdit = true;
            }
            DataContext = _category;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Добавление категории";
            }
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                MessageBox.Show("Введите название бренда");
            }
            else if (Validation.GetHasError(Name))
            {
                MessageBox.Show(
                    "Исправьте ошибки ввода перед сохранением",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            else
            {
                if (IsEdit)
                    service.Commit();
                else
                    service.Add(_category);
                Back(sender, e);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
