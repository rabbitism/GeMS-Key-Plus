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
using System.Text.RegularExpressions;

namespace GemsKeyPlus
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Tools : Window
    {
        private List<string> list = new List<string>();
        private string resultString = null;
        

        public Tools()
        {
            InitializeComponent();
        }

        public Tools(MainWindow window)
        {
            InitializeComponent();
            ResultBox.Text = window.stringForCommunication;
        }

        public string ConvertToQuickSearch(List<string> list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (list.Count>=1)
            {
                foreach (string number in list)
                {
                    stringBuilder.Append(",");
                    stringBuilder.Append(number);
                }
                stringBuilder.Remove(0, 1);
            }
            stringBuilder.Replace("Parts,", "", 0, 6);
            return stringBuilder.ToString();
        }

        private void String_Click(object sender, RoutedEventArgs e)
        {
            if(resultString!=null)
            {
                SplitNumbers(resultString);
            }            
            ResultBox.Text = ConvertToQuickSearch(list);
        }

        private void SplitNumbers(string text)
        {
            string[] Splitter = { " ", ",", ";", "\r", "\n", "\t" };
            //Set split option to remove empty entries to avoid opening empty pages.
            list = new List<string>(text.Split(Splitter, StringSplitOptions.RemoveEmptyEntries));
            list = list.Distinct().ToList();
        }

        private void ResultBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            resultString = ResultBox.Text;
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            if (resultString != null)
            {
                SplitNumbers(resultString);
            }
            ResultBox.Text = ConvertToBomUpload(list);
        }

        public string ConvertToBomUpload (List<string> list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Parts\r\n");
            if (list.Count >= 1)
            {
                foreach (string number in list)
                {
                    stringBuilder.Append(number);
                    stringBuilder.Append(",\r\n");
                }
                
            }
            stringBuilder.Replace("Parts,\r\n", "", 0, 8);
            return stringBuilder.ToString().TrimEnd();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetDataObject(ResultBox.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.FileName = "BOM Upload"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents|*.txt"; // Filter files by extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Nullable<bool> result = dlg.ShowDialog();
            if (dlg.FileName != "" && result==true)
            {
                using (StreamWriter sw = new StreamWriter(dlg.OpenFile()))
                {
                    sw.Write(ResultBox.Text);
                }
            }
        }

        private void ShowItem(object sender, RoutedEventArgs e)
        {
            //List<string> numbers = new List<string>();
            MatchCollection matches = Regex.Matches(InputBox.Text, @"(?<!\d)\d{9}(?!\S)|(?<!\d)\d{5}-\d{3}-\d{5}(?!\d)|(?<!\d)\d{9}D(?!\S)|(?<!\d)\d{9}d(?!\S)|(?<!\S)MDS-\d{1,3}(?!\d)|(?<!\S)QA-\d{1,2}(?!\d)|(?<!\S)CSP-\d{1,3}(?!\d)");
            list.Clear();
            foreach(Match s in matches)
            {
                list.Add(s.Value);
            }
            list = list.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            ResultList.Items.Clear();
            for(int i = 0; i< list.Count; i++)
            {
                ResultList.Items.Add(new Results { Count = i + 1, Valid = true, Number = list[i] });
            }
        }

        private void CopyList_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string number in list)
            {
                stringBuilder.Append(number + "\r\n");
            }
            Clipboard.SetDataObject(stringBuilder.ToString());
            
        }
    }

    class Results
    {
        public int Count { get; set; }
        public bool Valid { get; set; }
        public string Number{ get; set; }
    }
}
