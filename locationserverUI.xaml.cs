using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace locationserver
{
    /// <summary>
    /// Interaction logic for locationserverUI.xaml
    /// </summary>
    public partial class locationserverUI : UserControl
    {
        public static string CommandArgLogFile;
        public static string _CommandArgsSaveFile;

        public locationserverUI()
        {
            InitializeComponent();
        }

        public locationserverUI(string pCommandArgLogFile, string pCommandArgsSaveFile)
        {
            CommandArgLogFile = pCommandArgLogFile;
            _CommandArgsSaveFile = pCommandArgsSaveFile;

            InitializeComponent();
        }

        private void nSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<string> args = new List<string>();

            string logFile = nLogFileTextBox.Text;
            string saveFile = nSaveFileTextBox.Text;
            string timeout = nTimeoutTextBox.Text;

            //load
            if (logFile != "")
            {
                args.Add("-l");
                args.Add(logFile);
            }
            else if (CommandArgLogFile != null)
            {
                //use command line
                logFile = Program.LogFile;
                args.Add("-l");
                args.Add(logFile);
                nLogFileTextBox.Text = logFile; // inserted
            }

            // save
            if (saveFile != "")
            {
                args.Add("-f");
                args.Add(saveFile);
            }
            else if (_CommandArgsSaveFile != null)
            {
                //use command line
                saveFile = Program.SaveFile;
                args.Add("-f");
                args.Add(saveFile);
                nSaveFileTextBox.Text = saveFile; // inserted
            }

            //timeout
            if (timeout != "")
            {
                args.Add("-t");
                args.Add(timeout);
            }

            //debugging
            if (nDebugging.IsChecked == true)
            {
                args.Add("-d");
            }

            string[] input = new string[args.Count];
            for (int i = 0; i < args.Count; i++)
            {
                input[i] = args[i];
            }

            Output.SetLocationUi(this);

            Thread t = new Thread(() => Program.RunServer(input));
            t.Start();
        }
    }
}
