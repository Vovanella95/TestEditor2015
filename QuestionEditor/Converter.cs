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
            int pos = 0;
            int ind = str.IndexOf(separator);
            while (ind != -1)
            {
                if (ind == 0)
                {
                    yield return "";
                    pos = separator.Length;
                    ind = str.IndexOf(separator, pos + 1);
                    yield return str.Substring(pos, ind - pos);
                    pos = ind;
                    ind = str.IndexOf(separator, pos + 1);
                }
                else
                {
                    yield return str.Substring(pos + separator.Length, ind - pos - separator.Length);
                    pos = ind;
                    ind = str.IndexOf(separator, pos + 1);
                }
            }
            yield return str.Substring(pos + separator.Length);
        }

        /// <summary>
        /// Removing constructions like <tag> from input string>
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>formated string</returns>
        string RemoveTags(string str)
        {
            if (str == " .</")
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

            var inputWords = str.Split('\n').Where(w => !string.IsNullOrEmpty(w) && w[0] == 1).Select(w => new { Answer = (char)w[1], Text = w.Substring(3).Trim('\r') });
            XElement chun = new XElement("ChoiseUnit");
            choice.Add(chun);



            if (inputWords.Any(w => w.Answer == '1'))
            {
                ques.SetAttributeValue("NumberOfTexts", 1);
                foreach (var item in inputWords)
                {
                    var chun1 = new XElement("ChoiseUnit");
                    chun1.SetAttributeValue("Right", item.Answer == '0' ? "false" : "true");
                    chun1.SetAttributeValue("Text", RemoveTags(item.Text.Trim(((char)0x01))));
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
                    toq1.SetAttributeValue("Text", RemoveTags(textOfQuestion.Remove(index, textOfQuestion.IndexOf(">", index) - index + 1)));

                    var toqimg = new XElement("TextOfQuestion");
                    toqimg.SetAttributeValue("SymplyText", "");
                    toqimg.SetAttributeValue("Image", image);
                    toqimg.SetAttributeValue("Input", "");

                    toq.Add(toq1);
                    toq.Add(toqimg);
                }
                else
                {
                    toq1.SetAttributeValue("Text", RemoveTags(textOfQuestion));
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
                    toq1.SetAttributeValue("Text", RemoveTags(textOfQuestion.Remove(index, textOfQuestion.IndexOf(">", index) - index + 1)));

                    var toqimg = new XElement("TextOfQuestion");
                    toqimg.SetAttributeValue("SymplyText", "");
                    toqimg.SetAttributeValue("Image", image);
                    toqimg.SetAttributeValue("Input", "");
                    toq.Add(toqimg);
                }
                else
                {
                    toq1.SetAttributeValue("Text", RemoveTags(textOfQuestion));
                    toq1.SetAttributeValue("Image", "");
                }

                var text2 = str.Split('\n')[6];
                text2 = text2.Substring(1, text2.Length - 4);
                bool isInput = true;
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
            var d = demo[0];

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

            XElement root = new XElement("Root");
            foreach (var item in list)
            {
                root.Add(ParseString(item));
            }
            root.Save(outputFile);
        }
    }
}
