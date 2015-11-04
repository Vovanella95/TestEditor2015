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
    /// Interaction logic for Choise.xaml
    /// </summary>
    public partial class Choise : Window
    {
        public Choise()
        {
            InitializeComponent();
        }

        public List<string> Images = new List<string>();

        public string result;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var stackPannel = new StackPanel();
            var value = new ListBoxItem();
            value.Content = "False";
            value.Foreground = Brushes.White;
            value.Background = Brushes.Red;
            value.MouseUp += Value_MouseUp;

            var img = new ImgButton()
            {
                Content = "+",
                Width = 30,
                Foreground = Brushes.White,
                Background = Brushes.YellowGreen
            };
            img.Click += Img_Click;

            stackPannel.Children.Add(new Label() { Content = "Новый элемент" });
            stackPannel.Children.Add(value);
            stackPannel.Children.Add(img);
            stackPannel.Orientation = Orientation.Horizontal;

            var item = new ListBoxItem();
            item.Content = stackPannel;
            item.Selected += Item_Selected;

            choices.Items.Add(item);
            choices.SelectedItem = item;
        }

        private void Img_Click(object sender, RoutedEventArgs e)
        {
            var of = new System.Windows.Forms.OpenFileDialog();
            of.ShowDialog();
            if (string.IsNullOrEmpty(of.FileName)) return;
            ((ImgButton)sender).ImageUrl = of.FileName;
            ((ImgButton)sender).Background = Brushes.Green;
            Images.Add(of.FileName);
        }

        private string GetImageName(string src)
        {
            int i = src.Length - 1;
            while (src[i] != '\\') i--;
            return src.Substring(i + 1);
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            var item = (ListBoxItem)sender;
            var text = ((StackPanel)item.Content).Children[0];
            questionText.Text = (string)((Label)text).Content;
        }

        private void Value_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = (ListBoxItem)sender;
            if ((string)item.Content == "True")
            {
                item.Content = "False";
                item.Background = Brushes.Red;
            }
            else
            {
                item.Content = "True";
                item.Background = Brushes.Green;
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Label)((StackPanel)(((ListBoxItem)choices.SelectedItem).Content)).Children[0]).Content = questionText.Text;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            result = "[Choice\n";
            result += "[IsRandom = \"" + IsRandom.IsChecked + "\"]\n";
            foreach (ListBoxItem item in choices.Items)
            {
                result += "[Text = \"" + ((Label)((StackPanel)item.Content).Children[0]).Content + "\" ";
                result += "Value = \"" + ((ListBoxItem)((StackPanel)item.Content).Children[1]).Content + "\" ";
                if (!string.IsNullOrEmpty(((ImgButton)((StackPanel)item.Content).Children[2]).ImageUrl))
                {
                    result += "Image = \"" + GetImageName(((ImgButton)((StackPanel)item.Content).Children[2]).ImageUrl) + "\"]\n";
                }
                else
                {
                    result += "Image = \"\"";
                    result += "]";
                }
                result += "\n";
            }
            result += "]";
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        class ImgButton : Button
        {
            public string ImageUrl;
        }
    }
}
