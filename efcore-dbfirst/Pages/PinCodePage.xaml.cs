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
    /// Логика взаимодействия для PinCodePage.xaml
    /// </summary>
    public partial class PinCodePage : Page
    {
        bool manager = true;
        public string pinCode = "1234";
        public PinCodePage()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "ПИН-код";
            }
        }

        private void PinCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PinCode.MaxLength = 4;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PinCode.Password == pinCode)
                NavigationService.Navigate(new MainPage(manager));
            else if (PinCode.Password != pinCode)
                MessageBox.Show("Неверный ПИН-код", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
