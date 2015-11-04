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
using System.Windows.Shapes;
using System.IO;

namespace QuestionEditor
{
    /// <summary>
    /// Interaction logic for HowToUseIt.xaml
    /// </summary>
    public partial class HowToUseIt : Window
    {
        public HowToUseIt()
        {
            InitializeComponent();
            var a = Directory.GetCurrentDirectory() + "\\Data\\ForDevelopers.html";
            Browser.Navigate(a);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Browser.Navigate(Directory.GetCurrentDirectory() + "\\Data\\ForUsers.html");
            ForDeveloper.Foreground = Brushes.White;
            ForDeveloper.Background = Brushes.Green;

            ForUser.Foreground = Brushes.Gray;
            ForUser.Background = Brushes.White;
        }

        private void ForDeveloper_Click(object sender, RoutedEventArgs e)
        {
            Browser.Navigate(Directory.GetCurrentDirectory() + "\\Data\\ForDevelopers.html");
            ForUser.Foreground = Brushes.White;
            ForUser.Background = Brushes.Green;

            ForDeveloper.Foreground = Brushes.Gray;
            ForDeveloper.Background = Brushes.White;
        }
    }
}
