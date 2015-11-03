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
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Drawing;

namespace QuestionEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int NumberOfQuestions = 0;
        List<string> Images = new List<string>();
        string TestName;
        public List<string> States = new List<string>();
        public int CurrentStateIndex = 0;
        public int CurrentTextChanged = 0;
        public List<Tuple<string, string>> Preambula { get; set; }
        private string CurrentDirectory = null;
        private string CurrentFileName = null;

        #region Helpers

        public MainWindow()
        {
            InitializeComponent();
            FixProperties();

            Preambula = new List<Tuple<string, string>>();
            LoadPreambula();
            UpdateRemarks();
        }

        private void LoadPreambula()
        {
            Preambula.Clear();
            Preambula.Add(new Tuple<string, string>("Название теста", "Новый тест"));
            Preambula.Add(new Tuple<string, string>("Дата создания", DateTime.UtcNow.ToString()));
            Preambula.Add(new Tuple<string, string>("Темы, включенные в текст", "Тема1, Тема2, Тема3"));
        }

        private void UpdateRemarks()
        {
            Remarks.Items.Clear();
            foreach (var item in Preambula)
            {
                var textitem = new ListBoxItem();
                textitem.MouseUp += Textitem_MouseUp;
                textitem.Width = 189;
                var text = new TextBlock();
                text.Text = item.Item1 + ": " + item.Item2;
                text.TextWrapping = TextWrapping.Wrap;
                textitem.Content = text;
                Remarks.Items.Add(textitem);
            }
        }

        private void Textitem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var send = (TextBlock)((ListBoxItem)sender).Content;

            var el = Preambula.First(w => w.Item1 == SplitProperties(send.Text).Item1);

            var temp = SplitProperties(send.Text);
            Hider.Visibility = Visibility.Visible;
            var m = new Mark(temp.Item1, temp.Item2);
            m.ShowDialog();

            Hider.Visibility = Visibility.Hidden;
            Preambula = Preambula.Where(w => w.Item1 != el.Item1).ToList();
            Preambula.Add(new Tuple<string, string>(m.name, m.value));
            UpdateRemarks();
        }

        private Tuple<string, string> SplitProperties(string src)
        {
            int ind = src.IndexOf(":");
            return new Tuple<string, string>(src.Substring(0, ind), src.Substring(ind + 2));
        }

        private void FixProperties()
        {
            if (WindowState == WindowState.Maximized)
            {
                double h = SystemParameters.PrimaryScreenHeight;
                double w = SystemParameters.PrimaryScreenWidth;
                QuestionsStack.Height = h - 125;
                QuestionText.Height = h - 195;
                RemarksGrid.Height = h - 96;
                MiddleEditor.Width = w - 210 - 250 - 60;
                RemarksList.Height = h - 194;
            }
            else
            {
                QuestionsStack.Height = 768 - 155;
                QuestionText.Height = 543;
                RemarksGrid.Height = 646;
                MiddleEditor.Width = 505;
                RemarksList.Height = 547;
            }
        }

        #endregion

        #region Events

        private void MenuItem_Click_20(object sender, RoutedEventArgs e)
        {
            Hider.Visibility = Visibility.Visible;
            HowToUseIt a = new HowToUseIt();
            a.ShowDialog();
            Hider.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click_19(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.ShowDialog();

            if (string.IsNullOrEmpty(of.FileName))
            {
                return;
            }
            var converter = new Converter();
            try
            {
                converter.Dowork(of.FileName, of.FileName + ".xml");
                Hider.Visibility = Visibility.Visible;
                OkMessage yn = new OkMessage("Преобразовано успешно", "Файл сохранен как " + of.FileName + ".xml");
                yn.ShowDialog();
                Hider.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Hider.Visibility = Visibility.Visible;
                OkMessage yn = new OkMessage("Преобразование не удалось", "Ошибка анализа файла. Преобразование провалено, ошибка " + ex.Message);
                yn.ShowDialog();
                Hider.Visibility = Visibility.Hidden;
            }

        }

        bool hidden = false;
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!hidden)
            {
                QuestionsGrid.Margin = new Thickness(-250, 0, 0, 0);
                button3.Content = ">";
                MiddleEditor.Width += 260;
            }
            else
            {
                QuestionsGrid.Margin = new Thickness(10, 0, 0, 0);
                button3.Content = "<";
                MiddleEditor.Width -= 260;
            }
            hidden = !hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Hider.Visibility = Visibility.Visible;
            YesNo yn = new YesNo("Выход из приложения", "Вы действительно хотите выйти? Результат может быть утерян");
            yn.ShowDialog();
            Hider.Visibility = Visibility.Hidden;
            if (!yn.result)
            {
                return;
            }
            this.Close();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Hider.Visibility = Visibility.Visible;
            Authors a = new Authors();
            a.ShowDialog();
            Hider.Visibility = Visibility.Hidden;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var m = new Mark("название", "значение");
            m.ShowDialog();
            Preambula.Add(new Tuple<string, string>(m.name, m.value));
            UpdateRemarks();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            States.Clear();
            CurrentStateIndex = 0;


            NumberOfQuestions++;
            var question = new Question();
            question.Title = "Новый вопрос";
            question.Content = NumberOfQuestions + ". " + question.Title;
            question.Selected += Question_Selected;
            question.Number = NumberOfQuestions;
            question.Text = "[Difficulty = \"1\"]\nТекст вопроса";

            Questions.Items.Add(question);
            Questions.SelectedItem = question;

            MiddleEditor.IsEnabled = true;
        }

        private void Question_Selected(object sender, RoutedEventArgs e)
        {
            States.Clear();
            CurrentStateIndex = 0;

            Question selected = (Question)sender;
            QuestionText.Text = selected.Text;
            questionTitle.Text = selected.Title;
            QuestionNumber.Text = "Текущий вопрос " + selected.Number;
            QuestionCount.Text = "Всего вопросов " + NumberOfQuestions;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Question selected = (Question)Questions.SelectedItem;
            selected.Text = QuestionText.Text;
            selected.Title = questionTitle.Text;
            selected.Content = selected.Number + ". " + RemakeTitle(questionTitle.Text);
        }

        private void input_Click(object sender, RoutedEventArgs e)
        {
            int pos = QuestionText.CaretIndex;
            QuestionText.Text = QuestionText.Text.Insert(pos, "[Input Text=\"...\"]");
        }

        private string RemakeTitle(string str)
        {
            if (str.Length > 25)
            {
                return str.Substring(0, 25) + "...";
            }
            return str;
        }

        private void choise_Click(object sender, RoutedEventArgs e)
        {
            Hider.Visibility = Visibility.Visible;
            Choise choise = new Choise();
            choise.ShowDialog();
            Hider.Visibility = Visibility.Hidden;
            QuestionText.Text = QuestionText.Text.Insert(QuestionText.CaretIndex, choise.result);
        }

        private void image_Click(object sender, RoutedEventArgs e)
        {
            var od = new OpenFileDialog();
            od.Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|JPG Image (.jpg)|*.jpg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
            od.ShowDialog();
            QuestionText.Text += "\n[Image src=\"" + od.FileName + "\"]\n";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Questions.Items.Clear();
            Images.Clear();
            NumberOfQuestions = 0;
            LoadPreambula();
            UpdateRemarks();
            MiddleEditor.IsEnabled = false;
            FileTitle.Text = "Новый документ";
            CurrentFileName = null;
            CurrentDirectory = null;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
            FixProperties();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        #region Saving XML

        private XElement QuestionToElement(Question src, int code)
        {
            var elements = SplitText(src.Text).Select(w => w.Replace("\n", " ").Replace("\r", "")).Where(w => w != "").ToArray();
            XElement a = new XElement("Question");
            a.SetAttributeValue("Code", code);
            a.SetAttributeValue("Difficulty", 1);
            var toq = new XElement("TextOfQuestion");
            var choise = new XElement("Choice");
            int ind = 0;
            toq.Add(TextToString(src.Title, ind));
            ind++;

            foreach (var item in elements)
            {
                // If text is come
                if (item[0] != '[')
                {
                    toq.Add(TextToString(item, ind));
                    ind++;
                    continue;
                }

                //If input is come
                if (item.Substring(0, 6) == "[Input")
                {
                    toq.Add(InputToString(item, ind));
                }

                //If Image is come
                if (item.Substring(0, 6) == "[Image")
                {
                    toq.Add(ImageToString(item, ind));
                }


                if (item.Substring(0, 7) == "[Choice")
                {
                    choise.Add(ChoiseToString(item));
                    ind--;
                }

                if (item.Substring(0, 11) == "[Difficulty")
                {
                    a.SetAttributeValue("Difficulty", GetValue(item, "Difficulty"));
                    ind--;
                }

                ind++;
            }
            a.SetAttributeValue("NumberOfTexts", ind);
            a.Add(toq);
            a.Add(choise);
            return a;
        }

        private IEnumerable<string> SplitText(string src)
        {
            int ind = 0;
            while (src != "")
            {
                if (src[0] != '[')
                {
                    if (src.Contains('['))
                    {
                        yield return src.Substring(0, src.IndexOf('['));
                        src = src.Remove(0, src.IndexOf(('[')));
                        ind = 0;
                    }
                    else
                    {
                        yield return src;
                        break;
                    }

                }
                else
                {
                    ind = 1;
                    int tempCounter = 1;
                    while (tempCounter != 0)
                    {
                        if (src[ind] == '[') tempCounter++;
                        if (src[ind] == ']') tempCounter--;
                        ind++;
                    }
                    yield return src.Substring(0, ind);
                    src = src.Remove(0, ind);
                }
            }
        }

        private XElement InputToString(string src, int code)
        {
            var toq = new XElement("TextOfQuestion");
            toq.SetAttributeValue("SymplyText", "");
            toq.SetAttributeValue("Image", "");
            toq.SetAttributeValue("Code", code);
            toq.SetAttributeValue("Input", src.Substring(13, src.Length - 15));
            return toq;
        }

        private XElement ImageToString(string src, int code)
        {
            var imgSrc = src.Substring(12, src.Length - 14);
            Images.Add(imgSrc);
            int ind = imgSrc.Length - 1;
            while (imgSrc[ind] != '\\')
            {
                ind--;
            }

            var toq = new XElement("TextOfQuestion");
            toq.SetAttributeValue("SymplyText", "");
            toq.SetAttributeValue("Image", imgSrc.Substring(ind + 1));
            toq.SetAttributeValue("Input", "");
            toq.SetAttributeValue("Code", code);
            return toq;
        }

        private XElement TextToString(string src, int code)
        {
            var toq = new XElement("TextOfQuestion");
            toq.SetAttributeValue("SymplyText", src.Replace("\n", " "));
            toq.SetAttributeValue("Image", "");
            toq.SetAttributeValue("Input", "");
            toq.SetAttributeValue("Code", code);
            return toq;
        }

        private XElement ChoiseToString(string src)
        {
            var t = SplitText(src.Substring(1)).Where(w => w[0] == '[').ToArray();
            var choiceUnit = new XElement("ChoiseUnit");
            choiceUnit.SetAttributeValue("IsRandom", GetValue(t[0], "IsRandom"));

            foreach (var item in t.Skip(1))
            {
                choiceUnit.Add(ChoiseItemToString(item));
            }
            return choiceUnit;
        }

        private string GetValue(string src, string name)
        {
            int ind = src.IndexOf('\"', src.IndexOf(name));
            return src.Substring(ind + 1, src.IndexOf('\"', ind + 1) - ind - 1);
        }

        private XElement ChoiseItemToString(string src)
        {
            var choiseUnit = new XElement("ChoiceUnit");
            choiseUnit.SetAttributeValue("Text", GetValue(src, "Text"));
            choiseUnit.SetAttributeValue("Value", GetValue(src, "Value"));
            return choiseUnit;
        }

        private void WriteImages(string url)
        {
            int ind2 = url.Length - 1;
            while (url[ind2] != '\\')
            {
                ind2--;
            }
            string testname = url.Substring(ind2 + 1);


            ind2 = testname.IndexOf('.');

            string newurl = url.Replace(testname, "");

            TestName = testname.Substring(0, ind2);
            Directory.CreateDirectory(newurl + "\\" + testname.Substring(0, ind2));

            foreach (var image in Images)
            {
                int ind = image.Length - 1;
                while (image[ind] != '\\')
                {
                    ind--;
                }
                string filename = image.Substring(ind + 1);

                ind = image.Length - 1;
                while (image[ind] != '\\')
                {
                    ind--;
                }
                File.Copy(image, newurl + "\\" + TestName + "\\" + filename, true);
            }
        }

        private void MenuItem_Click_18(object sender, RoutedEventArgs e)
        {
            if (Questions.Items.Count == 0)
            {
                Hider.Visibility = Visibility.Visible;
                OkMessage yn = new OkMessage("Сохранение не удалось", "Тест не имеет ни одного вопроса. Такое сохранение не допустимо");
                yn.ShowDialog();
                Hider.Visibility = Visibility.Hidden;
                return;
            }
            var test = new XElement("Test");
            var sd = new SaveFileDialog();
            sd.Filter = "XML documents | *.xml";
            sd.ShowDialog();
            if (string.IsNullOrEmpty(sd.FileName)) return;
            test.SetAttributeValue("TestName", sd.FileName);

            var preambula = new XElement("Preambula");
            foreach (var item in Preambula)
            {
                var temp = new XElement("PreambulaItem");
                temp.SetAttributeValue("Name", item.Item1);
                temp.SetAttributeValue("Value", item.Item2);
                preambula.Add(temp);
            }
            test.Add(preambula);

            var question = new XElement("Question");
            int ind = 0;
            foreach (Question item in Questions.Items)
            {
                question.Add(QuestionToElement(item, ind));
                ind++;
            }
            test.SetAttributeValue("NumberOfQuestions", ind);
            test.Add(question);

            test.Save(sd.FileName);
            WriteImages(sd.FileName);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (Questions.Items.Count == 0)
            {
                Hider.Visibility = Visibility.Visible;
                OkMessage yn = new OkMessage("Сохранение не удалось", "Тест не имеет ни одного вопроса. Такое сохранение не допустимо");
                yn.ShowDialog();
                Hider.Visibility = Visibility.Hidden;
                return;
            }

            var test = new XElement("Test");
            string filename = CurrentDirectory;
            if (string.IsNullOrEmpty(CurrentDirectory))
            {
                var sd = new SaveFileDialog();
                sd.Filter = "XML documents | *.xml";
                sd.ShowDialog();
                if (string.IsNullOrEmpty(sd.FileName)) return;
                test.SetAttributeValue("TestName", sd.FileName);
                filename = sd.FileName;
                CurrentFileName = filename;
            }

            var preambula = new XElement("Preambula");
            foreach (var item in Preambula)
            {
                var temp = new XElement("PreambulaItem");
                temp.SetAttributeValue("Name", item.Item1);
                temp.SetAttributeValue("Value", item.Item2);
                preambula.Add(temp);
            }
            test.Add(preambula);

            var question = new XElement("Question");
            int ind = 0;
            foreach (Question item in Questions.Items)
            {
                question.Add(QuestionToElement(item, ind));
                ind++;
            }
            test.SetAttributeValue("NumberOfQuestions", ind);
            test.Add(question);

            test.Save(CurrentFileName);

            WriteImages(CurrentFileName);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Hider.Visibility = Visibility.Visible;
            YesNo yn = new YesNo("Удалить вопрос?", "Вы уверены в том, что хотите удалить этот вопрос? Отменить это будет невозможно");
            yn.ShowDialog();
            Hider.Visibility = Visibility.Hidden;
            if (!yn.result)
            {
                return;
            }

            Questions.Items.Remove(Questions.SelectedItem);
            if (Questions.Items.Count != 0)
            {
                Questions.SelectedItem = Questions.Items[0];
                int ind = 1;
                foreach (Question item in Questions.Items)
                {
                    string old = (string)item.Content;
                    item.Number = ind;
                    item.Content = ind + ". " + old.Substring(old.IndexOf(' ') + 1);
                    ind++;
                }
                NumberOfQuestions = ind - 1;
            }
            else
            {
                MiddleEditor.IsEnabled = false;
                NumberOfQuestions = 0;
            }
        }

        #endregion

        #region Preview

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            var tempFileName = Directory.GetCurrentDirectory();
            var test = new XElement("Test");
            test.SetAttributeValue("TestName", tempFileName + "test.xml");

            var question = new XElement("Question");
            int ind = 0;
            foreach (Question item in Questions.Items)
            {
                question.Add(QuestionToElement(item, ind));
                ind++;
            }
            test.SetAttributeValue("NumberOfQuestions", ind);
            test.Add(question);
            test.Save(tempFileName + "test.xml");
            WriteImages(tempFileName + "test.xml");
            var text = XmlToHtml(tempFileName + "test.xml");

            using (var sw = new StreamWriter(tempFileName + "test.html"))
            {
                sw.Write(text);
            }

            File.Copy(Directory.GetCurrentDirectory() + "\\Data\\style.css", tempFileName + "test\\style.css", true);

            System.Diagnostics.Process.Start(tempFileName + "test.html");
        }

        private string XmlToHtml(string path)
        {
            XElement root = XElement.Load(path);
            string html = string.Empty;
            var questions = root.Descendants("Question").Skip(1).ToArray();

            int ind = 1;
            foreach (var item in questions)
            {
                html += QuestionToHtml(item, ind);
                ind++;
            }
            return "<html><head><meta charset=\"UTF-8\"><link rel=\"stylesheet\" type=\"text/css\" href=\"" + TestName + "/style.css\"></head><body style=\"background:#DDDDDD\"><header>TEST - предпросмотр</header> " + html + " </body></html>";
        }

        private string QuestionToHtml(XElement question, int num)
        {
            string codeBegin = "<div class=\"example\">\n";
            string codeEnd = "</div>\n\n\n";

            //header
            string codeHeader = "<div class=\"header\">" + num + ". " + question.Descendants("TextOfQuestion").ToArray()[1].Attribute("SymplyText").Value + " </div>\n\n";
            //text of question
            string codeToq = "<div class=\"textofquestion\">\n";
            foreach (var item in question.Descendants("TextOfQuestion").Skip(2))
            {
                codeToq += ToqToHtml(item);
            }
            codeToq += "</div>\n";

            //Choise
            string codeChoise = "";
            if (question.Element("Choice").Element("ChoiseUnit") != null)
            {
                codeChoise = ChoiseToHtml(question.Element("Choice"));
            }

            // submitter
            string codeSubmiter = "<div class=\"submitter\"><center><input type=\"submit\" value=\"Submit\"/></center></div>\n";

            return codeBegin + codeHeader + codeToq + codeChoise + codeSubmiter + codeEnd;

        }

        private string ToqToHtml(XElement textOfQuestion)
        {
            // for text
            if (textOfQuestion.Attribute("SymplyText").Value != string.Empty)
            {
                return textOfQuestion.Attribute("SymplyText").Value;
            }

            // for input
            if (textOfQuestion.Attribute("Input").Value != string.Empty)
            {
                return "\t<input type=\"text\" class=\"toqsubmit\" Value=\"\"/>";
            }

            // for image
            if (textOfQuestion.Attribute("Image").Value != string.Empty)
            {
                return "\n\t<br/>\n\t\t<image src=\"" + TestName + "/" + textOfQuestion.Attribute("Image").Value + "\" Height=\"100\" class=\"toqimage\" />\n\t<br/>\n";
            }

            throw new Exception("Simething bad");
        }

        private string ChoiseToHtml(XElement choise)
        {
            string ch = "\n<div class=\"choiser\">";

            foreach (var item in choise.Elements("ChoiseUnit").Elements())
            {
                ch += "<input type=\"checkbox\">" + item.Attribute("Text").Value + "</br>\n";
            }
            return ch + "</div>\n";
        }

        #endregion

        #region Redacting Stuff

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            AddTag("<b>", "</b>");
        }

        private void AddTag(string begin, string end)
        {
            int ind1 = QuestionText.SelectionStart;
            int ind2 = QuestionText.SelectionLength;

            var text = QuestionText.Text;
            var inside = text.Substring(ind1, ind2);
            text = text.Remove(ind1, ind2);

            QuestionText.Text = text.Insert(ind1, begin + inside + end);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='text-decoration: underline'>", "</label>");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            AddTag("<i>", "</i>");
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='font-size:12px'>", "</label>");
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='font-size:14px'>", "</label>");
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='font-size:16px'>", "</label>");
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='font-size:16px'>", "</label>");
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            Hider.Visibility = Visibility.Visible;
            var fs = new FontSelector();
            fs.ShowDialog();
            Hider.Visibility = Visibility.Hidden;

            if (fs.Success)
            {
                AddTag("<label style='font-size:" + fs.FS + "px'>", "</label>");
            }
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            var cd = new ColorDialog();
            cd.ShowDialog();

            AddTag("<label style='color: rgb(" + cd.Color.R + "," + cd.Color.G + "," + cd.Color.B + ")'>", "</label>");
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='color: black'>", "</label>");
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='color: #B22222'>", "</label>");
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='color: #1E90FF'>", "</label>");
        }

        private void MenuItem_Click_14(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='color: #2E8B57'>", "</label>");
        }

        private void MenuItem_Click_15(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='color: #8A2BE2'>", "</label>");
        }

        private void MenuItem_Click_16(object sender, RoutedEventArgs e)
        {
            AddTag("<label style='color: #D2691E'>", "</label>");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (CurrentStateIndex != 0)
            {
                if (CurrentStateIndex == States.Count)
                {
                    States.Add(QuestionText.Text);
                    CurrentStateIndex += 1;
                }
                QuestionText.Text = States.ElementAt(CurrentStateIndex - 1);
                CurrentStateIndex -= 1;
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (CurrentStateIndex < States.Count - 1)
            {
                QuestionText.Text = States.ElementAt(CurrentStateIndex + 1);
                CurrentStateIndex += 1;
            }
        }

        private void QuestionText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (States.Contains(QuestionText.Text))
            {
                return;
            }
            if (e.Changes.Count == 1 && e.Changes.ElementAt(0).AddedLength == 1)
            {
                CurrentTextChanged += 1;
            }
            if (e.Changes.Count > 1
                || e.Changes.ElementAt(0).AddedLength > 1
                || e.Changes.ElementAt(0).RemovedLength > 1
                || CurrentTextChanged > 10)
            {
                FillStates();
                States.Add(QuestionText.Text);
                CurrentStateIndex = States.Count;
            }

            if (CurrentTextChanged > 10)
            {
                CurrentTextChanged = 0;
            }
        }

        private void FillStates()
        {
            if (States.Count > 200)
            {
                States = States.Skip(50).ToList();
                CurrentStateIndex = States.Count;
            }
        }

        #endregion

        #region OpenFile

        private void MenuItem_Click_17(object sender, RoutedEventArgs e)
        {
            var od = new OpenFileDialog();
            od.ShowDialog();

            if (string.IsNullOrEmpty(od.FileName))
            {
                return;
            }

            Preambula.Clear();
            UpdateRemarks();
            Questions.Items.Clear();
            XElement root = null;

            try
            {
                root = XElement.Load(od.FileName);
            }
            catch (Exception ex)
            {
                Hider.Visibility = Visibility.Visible;
                OkMessage yn = new OkMessage("Некорректный файл", "Файл, который вы пытаетесь загрузить, загружен некорректно, ошибка \"" + ex.Message + "\"");
                yn.ShowDialog();
                Hider.Visibility = Visibility.Hidden;
                return;
            }

            CurrentFileName = od.FileName;
            CurrentDirectory = od.FileName;
            int ind = CurrentDirectory.Length - 1;
            while (CurrentDirectory[ind] != '\\')
            {
                ind--;
            }
            CurrentDirectory = CurrentDirectory.Substring(0, ind + 1);

            LoadQuestions(root);
            LoadRemarks(root.Element("Preambula"));

            if (Questions.Items.Count == 0)
            {
                MiddleEditor.IsEnabled = false;
            }
            FileTitle.Text = CurrentFileName;
        }

        private void LoadQuestions(XElement root)
        {
            NumberOfQuestions = 0;
            var questions = root.Descendants("Question").Skip(1).ToArray();
            foreach (var q in questions)
            {
                NumberOfQuestions++;
                var question = XmlToQuestion(q, NumberOfQuestions);
                Questions.Items.Add(question);
                Questions.SelectedItem = question;
            }

            MiddleEditor.IsEnabled = true;
        }

        private Question XmlToQuestion(XElement q, int id)
        {
            var question = new Question();
            question.Title = q.Element("TextOfQuestion").Element("TextOfQuestion").Attribute("SymplyText").Value;
            question.Difficulty = Convert.ToInt32(q.Attribute("Difficulty").Value);
            question.Content = NumberOfQuestions + ". " + question.Title;
            question.Selected += Question_Selected;
            var text = "[Difficulty = \"" + question.Difficulty + "\"]\n";


            //Adding toq
            var toq = q.Element("TextOfQuestion").Elements().Skip(1).ToArray();
            foreach (var item in toq)
            {
                text += ToqToText(item);
            }

            // Adding choice
            text += ChoiseToText(q.Element("Choice"));

            question.Text = text;
            return question;
        }

        private string ToqToText(XElement toq)
        {
            // if toq is text
            if (!string.IsNullOrEmpty(toq.Attribute("SymplyText").Value))
            {
                return "\n" + toq.Attribute("SymplyText").Value + "\n";
            }

            // of toq is image
            if (!string.IsNullOrEmpty(toq.Attribute("Image").Value))
            {
                var dirName = CurrentFileName;
                int i = dirName.Length - 1;
                while(dirName[i]!='\\')
                {
                    i--;
                }
                dirName = dirName.Substring(i);
                dirName = dirName.Substring(0, dirName.IndexOf('.'));

                Images.Add(CurrentDirectory + toq.Attribute("Image").Value);
                return "\n[Image src=\"" + CurrentDirectory + dirName.Replace("\\","") + "\\" + toq.Attribute("Image").Value + "\"]\n";
            }

            // of toq is input
            if (!string.IsNullOrEmpty(toq.Attribute("Input").Value))
            {
                return "\n[Input Text=\"" + toq.Attribute("Input").Value + "\"]\n";
            }
            throw new Exception("Unknown element");
        }

        private string ChoiseToText(XElement choice)
        {
            if (!choice.HasElements)
            {
                return "";
            }
            string text = "[Choice\n";

            text += "[IsRandom = \"" + choice.Element("ChoiseUnit").Attribute("IsRandom").Value + "\"]\n";

            foreach (var item in choice.Element("ChoiseUnit").Elements())
            {
                text += "[Text = \"" + item.Attribute("Text").Value + "\" Value = \"" + item.Attribute("Value").ToString() + "]\n";
            }
            return text + "]\n";
        }

        private void LoadRemarks(XElement preambula)
        {
            foreach (var item in preambula.Elements())
            {
                Preambula.Add(new Tuple<string, string>(item.Attribute("Name").Value, item.Attribute("Value").Value));
                UpdateRemarks();
            }
        }

        #endregion

    }



    public class Question : ListBoxItem
    {
        public string Title;
        public int Difficulty;
        public string Text;
        public int Number;
    }


}
