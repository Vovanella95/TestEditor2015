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

namespace QuestionEditor
{
    /// <summary>
    /// Interaction logic for YesNo.xaml
    /// </summary>
    public partial class YesNo : Window
    {
        public bool result;
        public YesNo(string title, string text)
        {
            InitializeComponent();
            Title = title;
            textBlock.Text = text;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            Close();
        }
    }
}
