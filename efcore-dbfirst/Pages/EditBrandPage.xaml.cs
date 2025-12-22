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
    /// Логика взаимодействия для EditBrandPage.xaml
    /// </summary>
    public partial class EditBrandPage : Page
    {
        Brand _brand = new();
        BrandService service = new();
        bool IsEdit = false;
        public EditBrandPage(Brand? brand = null)
        {
            InitializeComponent();

            if (brand != null)
            {
                _brand = brand;
                IsEdit = true;
            }
            DataContext = _brand;
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Редактирование бренда";
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                service.Commit();
            else
                service.Add(_brand);
            Back(sender, e);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_brand != null)
                if (MessageBox.Show(
                    "Вы действительно хотите удалить это?",
                    "Удалить группу?",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(_brand);
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
    }
}
