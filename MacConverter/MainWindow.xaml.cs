using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MacConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string RemoveUnwantedCharacters(string input, IEnumerable<char> allowedCharacters)
        {
            {
                var filtered = input.ToCharArray()
                    .Where(c => allowedCharacters.Contains(c))
                    .ToArray();

                return new String(filtered);
            }
        }

        public string inputMac { get; set; }

        public string outputMac { get; set; }

        public string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" ;

        private async void none_to_2dash(object obj)

        {
            await Dispatcher.BeginInvoke(new Action(() => {
                inputMac = macTxt.Text;
                string filteredString = RemoveUnwantedCharacters(inputMac, allowedCharacters);
                filteredString = filteredString.Insert(2, "-");
                filteredString = filteredString.Insert(5, "-");
                filteredString = filteredString.Insert(8, "-");
                filteredString = filteredString.Insert(11, "-");
                filteredString = filteredString.Insert(14, "-");
                outputMac = filteredString;
                resultTxt.Text = outputMac;
            }));
        }

        private async void none_to_2colon(object obj)
        {
            await Dispatcher.BeginInvoke(new Action(() => {
                inputMac = macTxt.Text;
                string filteredString = RemoveUnwantedCharacters(inputMac, allowedCharacters);
                filteredString = filteredString.Insert(2, ":");
                filteredString = filteredString.Insert(5, ":");
                filteredString = filteredString.Insert(8, ":");
                filteredString = filteredString.Insert(11, ":");
                filteredString = filteredString.Insert(14, ":");
                outputMac = filteredString;
                resultTxt.Text = outputMac;
            }));
        }
        private async void none_to_4dot(object obj)
        {
            await Dispatcher.BeginInvoke(new Action(() => {
                inputMac = macTxt.Text;
                string filteredString = RemoveUnwantedCharacters(inputMac, allowedCharacters);
                filteredString = filteredString.Insert(4, ".");
                filteredString = filteredString.Insert(9, ".");
                outputMac = filteredString;
                resultTxt.Text = outputMac;
            }));
        }

        private async void ConvertSingle_Click(object sender, RoutedEventArgs e)
        {

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                AnimateShow = false,
                AnimateHide = false
            };

            if (string.IsNullOrEmpty(macTxt.Text))
            {
                var result = await this.ShowMessageAsync("Error",
                    "Missing MAC, check data entry",
                    MessageDialogStyle.Affirmative, mySettings);
                return;
            }

            if (radDash.IsChecked == true)
            {
                Thread thread = new Thread(none_to_2dash);
                thread.IsBackground = true;
                thread.Start();
            }

            if (radColon.IsChecked == true)
            {
                Thread thread = new Thread(none_to_2colon);
                thread.IsBackground = true;
                thread.Start();
            }

            if (rad4Dot.IsChecked == true)
            {
                Thread thread = new Thread(none_to_4dot);
                thread.IsBackground = true;
                thread.Start();
            }

        }



        #region Batch Grid

        private string _sourcefile = null;

        private string _outfile = null;


        public string sourcefile
        {
            get { return _sourcefile; }
            set { _sourcefile = value; }
        }

        public string outfile
        {
            get { return _outfile; }
            set { _outfile = value; }
        }

        private void Browser1_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            DialogResult result = FileDialog.ShowDialog();
            if (result.ToString() == "OK")
                _sourcefile = FileDialog.FileName;
            srcLabel.Content = sourcefile;
        }

        private void Browser2_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            DialogResult result = FileDialog.ShowDialog();
            if (result.ToString() == "OK")
                _outfile = FileDialog.FileName;
            outLabel.Content = outfile;
        }

        private void Convert_macs_button_Click(object sender, RoutedEventArgs e)
        {
            

            List<string> results = new List<string>();

            foreach (string line in File.ReadLines(sourcefile))
            {
                if (radDash2.IsChecked == true)
                {
                    inputMac = line;
                    string filteredString = RemoveUnwantedCharacters(inputMac, allowedCharacters);
                    filteredString = filteredString.Insert(2, "-");
                    filteredString = filteredString.Insert(5, "-");
                    filteredString = filteredString.Insert(8, "-");
                    filteredString = filteredString.Insert(11, "-");
                    filteredString = filteredString.Insert(14, "-");
                    outputMac = filteredString;
                    results.Add(outputMac);
                    
                }

                if (radColon2.IsChecked == true)
                {
                    inputMac = line;
                    string filteredString = RemoveUnwantedCharacters(inputMac, allowedCharacters);
                    filteredString = filteredString.Insert(2, ":");
                    filteredString = filteredString.Insert(5, ":");
                    filteredString = filteredString.Insert(8, ":");
                    filteredString = filteredString.Insert(11, ":");
                    filteredString = filteredString.Insert(14, ":");
                    outputMac = filteredString;
                    results.Add(outputMac);
                }

                if (rad4Dot2.IsChecked == true)
                {
                    inputMac = line;
                    string filteredString = RemoveUnwantedCharacters(inputMac, allowedCharacters);
                    filteredString = filteredString.Insert(4, ".");
                    filteredString = filteredString.Insert(9, ".");
                    outputMac = filteredString;
                    results.Add(outputMac);
                }
            }

            System.IO.File.WriteAllLines(outfile, results);

            string sometext = System.IO.File.ReadAllText(outfile);

            resultBoxTxt.Text = sometext;

        }


        #endregion
    }
}
