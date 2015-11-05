using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace QuestionEditor
{
    public class Converter
    {
        /// <summary>
        /// Spliting string by inputed sepatator (Do not rusifyed me!)
        /// </summary>
        /// <param name="str">source string</param>
        /// <param name="separator">separator</param>
        /// <returns>returns collection of strings</returns>
        IEnumerable<string> Split(string str, string separator)
        {
            return str.Split(new string[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Removing constructions like <tag> from input string>
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>formated string</returns>
        string RemoveTags(string str)
        {
            if (str == " .</" || str == " </P")
            {
                return "";
            }
            int cur = 0;
            var k = str;
            int ind = k.IndexOf("<");
            while (ind != -1)
            {
                int ind2 = k.IndexOf(">", ind);
                if (k.Substring(ind, 4) == "<IMG")
                {
                    cur = ind + 1;
                    ind = k.IndexOf("<", cur);
                    continue;
                }
                k = k.Remove(ind, ind2 - ind + 1);
                ind = k.IndexOf("<", cur);
            }

            return k.Replace("&nbsp;", " ")
                    .Replace((char)0x12, ' ')
                    .Replace("\r", "");
        }

        /// <summary>
        /// Transform string to XElement
        /// </summary> (There's no cow level)
        /// <param name="str">source string</param>
        /// <returns>XElement representation of string</returns>
        XElement ParseString(string str)
        {
            var ques = new XElement("Question");
            ques.SetAttributeValue("Difficulity", "1");
            ques.SetAttributeValue("Code", Convert.ToInt32(str.Split('\n').First().Substring(1)) - 1);

            var toq = new XElement("TextOfQuestion");
            var choice = new XElement("Choice");

            ques.Add(toq);
            ques.Add(choice);

            var inputWords = str.Split('\n').Where(w => !string.IsNullOrEmpty(w) && w[0] == 1).Select(w => new { Answer = w[1], Text = w.Substring(3).Trim('\r') });
            XElement chun = new XElement("ChoiseUnit");
            choice.Add(chun);

            if (inputWords.Any(w => w.Answer == '1'))
            {
                ques.SetAttributeValue("NumberOfTexts", 1);
                foreach (var item in inputWords)
                {
                    var chun1 = new XElement("ChoiseUnit");
                    chun1.SetAttributeValue("Value", item.Answer == '0' ? "false" : "true");
                    chun1.SetAttributeValue("Text", RemoveTags(item.Text.Trim(((char)0x01))));

                    var t = RemoveTags(item.Text.Trim(((char)0x01)));
                    var text = item.Text;
                    if (text.Contains("<IMG"))
                    {
                        int ind = text.IndexOf("src=");
                        while (text[ind] != '\"') ind++;
                        ind++;

                        int ind2 = ind + 1;
                        while (text[ind2] != '\"') ind2++;

                        var img = text.Substring(ind, ind2 - ind);
                        chun1.SetAttributeValue("Image", img);

                        var txt = RemoveTags(item.Text.Trim(((char)0x01)));
                        txt = txt.Replace(txt.Substring(txt.IndexOf('<'), txt.IndexOf('>') - txt.IndexOf('<') + 1), "");
                        chun1.SetAttributeValue("Text", txt);
                    }
                    chun.Add(chun1);
                }
                var textOfQuestion = str.Split('\n')[4];
                var toq1 = new XElement("TextOfQuestion");
                toq1.SetAttributeValue("Code", "0");
                if (textOfQuestion.Contains("<IMG"))
                {
                    int index = textOfQuestion.IndexOf("src=\"") + 5;
                    string image = textOfQuestion.Substring(index, textOfQuestion.IndexOf('\"', index) - index);
                    index = textOfQuestion.IndexOf("<IMG");
                    toq1.SetAttributeValue("SymplyText", RemoveTags(textOfQuestion.Remove(index, textOfQuestion.IndexOf(">", index) - index + 1)));

                    var toqimg = new XElement("TextOfQuestion");
                    toqimg.SetAttributeValue("SymplyText", "");
                    toqimg.SetAttributeValue("Image", image);
                    toqimg.SetAttributeValue("Input", "");

                    toq.Add(toq1);
                    toq.Add(toqimg);
                }
                else
                {
                    toq1.SetAttributeValue("SymplyText", RemoveTags(textOfQuestion));
                    toq1.SetAttributeValue("Image", "");
                    toq.Add(toq1);
                }
            }
            else
            {
                var textOfQuestion = str.Split('\n')[4];
                var toq1 = new XElement("TextOfQuestion");
                toq1.SetAttributeValue("Code", "0");
                toq.Add(toq1);
                toq1.SetAttributeValue("Input", "");
                if (textOfQuestion.Contains("<IMG"))
                {
                    int index = textOfQuestion.IndexOf("src=\"") + 5;
                    string image = textOfQuestion.Substring(index, textOfQuestion.IndexOf('\"', index) - index);
                    index = textOfQuestion.IndexOf("<IMG");
                    toq1.SetAttributeValue("SymplyText", RemoveTags(textOfQuestion.Remove(index, textOfQuestion.IndexOf(">", index) - index + 1)));

                    var toqimg = new XElement("TextOfQuestion");
                    toqimg.SetAttributeValue("SymplyText", "");
                    toqimg.SetAttributeValue("Image", image);
                    toqimg.SetAttributeValue("Input", "");
                    toq.Add(toqimg);
                }
                else
                {
                    toq1.SetAttributeValue("SymplyText", RemoveTags(textOfQuestion));
                    toq1.SetAttributeValue("Image", "");
                }

                var text2 = str.Split('\n')[6];
                text2 = text2.Substring(1, text2.Length - 4);
                bool isInput = text2.Substring(0, 4) == "<%s>";
                int code = 1;
                int numberOfInputWord = 0;
                int numberOfQues = 0;
                var words = Split(text2, "<%s>").Select(w => RemoveTags(w)).Where(w => w != "");

                while (true)
                {
                    if (isInput)
                    {
                        if (inputWords.Count() == numberOfInputWord)
                        {
                            break;
                        }
                        var toq2 = new XElement("TextOfQuestion");
                        toq2.SetAttributeValue("Code", code);
                        toq2.SetAttributeValue("Image", "");
                        toq2.SetAttributeValue("Input", inputWords.ElementAt(numberOfInputWord).Text);
                        numberOfInputWord++;
                        toq.Add(toq2);
                        code++;
                    }
                    else
                    {
                        if (words.Count() == numberOfQues)
                        {
                            break;
                        }
                        var toq2 = new XElement("TextOfQuestion");
                        toq2.SetAttributeValue("Code", code);
                        toq2.SetAttributeValue("SymplyText", words.ElementAt(numberOfQues));
                        toq2.SetAttributeValue("Input", "");
                        toq2.SetAttributeValue("Image", "");
                        numberOfQues++;
                        toq.Add(toq2);
                        code++;
                    }
                    isInput = !isInput;
                }
                ques.SetAttributeValue("NumberOfTexts", code);
            }
            return ques;
        }

        /// <summary>
        /// Reading file and converting this to XML format
        /// </summary>
        /// <param name="inputFile">input file path</param>
        /// <param name="outputFile">output file path</param>
        public void Dowork(string inputFile, string outputFile)
        {
            var demo = new StreamReader(inputFile).ReadToEnd();

            List<string> list = new List<string>();

            int ind = demo.IndexOf((char)14);
            int ind2 = demo.IndexOf((char)11, ind);

            while (ind != -1)
            {
                list.Add(demo.Substring(ind, ind2 - ind + 1));
                ind = demo.IndexOf((char)14, ind + 1);
                if (ind == -1) break;

                ind2 = demo.IndexOf((char)11, ind);
            }
            XElement root = new XElement("Test");

            var preambula = new XElement("Preambula");
            var title = new XElement("PreambulaItem");
            title.SetAttributeValue("Name", "Название");

            var tt = demo.Split('\n').ElementAt(1).Replace((char)18, ' ').Trim();
            title.SetAttributeValue("Value", tt);

            var theme = new XElement("PreambulaItem");
            theme.SetAttributeValue("Name", "Тема");

            var ttt = demo.Split('\n').ElementAt(2).Replace((char)19, ' ').Trim();
            theme.SetAttributeValue("Value", ttt);

            preambula.Add(title);
            preambula.Add(theme);
            root.Add(preambula);


            foreach (var item in list)
            {
                root.Add(ParseString(item));
            }
            root.Save(outputFile);
        }
    }
}
