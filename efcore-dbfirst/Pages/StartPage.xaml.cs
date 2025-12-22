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
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        bool manager = false;
        public StartPage()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Title = "Выбор роли";
            }
        }

        private void UserLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(manager));
        }

        private void ManagerLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PinCodePage());
        }
    }
}
