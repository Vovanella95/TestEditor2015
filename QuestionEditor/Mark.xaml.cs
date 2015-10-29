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
    /// Interaction logic for Mark.xaml
    /// </summary>
    public partial class Mark : Window
    {
        public string name;
        public string value;

        public Mark(string name, string value)
        {
            InitializeComponent();
            textBox.Text = name;
            textBox1.Text = value;
            this.name = name;
            this.value = value;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            name = textBox.Text;
            value = textBox1.Text;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
