using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.Windows.Threading;

namespace FileEncryption
{
    public partial class MainWindow : Window
    {
        private BackgroundWorker work;


        private string inputFile;
        private string outputFile;
        bool fileTry;       
        private string key;

        public MainWindow()
        {
            InitializeComponent();

            work = new BackgroundWorker();
            work.WorkerReportsProgress = true;
            work.ProgressChanged += Worker_ProgressChanged;
            work.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void InputFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                InputFileTextBox.Text = openFileDialog.FileName;
        }

        private void OutputFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new CommonOpenFileDialog();
            openFile.IsFolderPicker = true;

            if (openFile.ShowDialog() == CommonFileDialogResult.Ok)
                OutputFileTextBox.Text = openFile.FileName;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }
        private void ValidtateButton_Click(object sender, RoutedEventArgs e)
        {
            key = KeyTextBox.Text;
            inputFile = InputFileTextBox.Text;
            outputFile = OutputFileTextBox.Text;

            if (((Button)sender).Name == "EncryptionValidtateButton")
                work.DoWork += Encryption;
            else work.DoWork += Decryption;

            LockWindowsInterface();
            work.RunWorkerAsync(KeyTextBox.Text);
        }
        
        private void LockWindowsInterface()
        {
            InputFileTextBox.IsReadOnly = true;
            OutputFileTextBox.IsReadOnly = true;
            InputFileButton.IsEnabled = false;
            OutputFileButton.IsEnabled = false;
            EncryptionValidtateButton.IsEnabled = false;
            DecryptionValidtateButton.IsEnabled = false;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Value = 0;
            UnlockWindowsInterface();
            work.DoWork -= Encryption;
            work.DoWork -= Decryption;
            
            if (fileTry) return;
            ShowMessage();
        }

        private void UnlockWindowsInterface()
        {
            InputFileTextBox.IsReadOnly = false;
            OutputFileTextBox.IsReadOnly = false;
            InputFileButton.IsEnabled = true;
            OutputFileButton.IsEnabled = true;
            EncryptionValidtateButton.IsEnabled = true;
            DecryptionValidtateButton.IsEnabled = true;
        }
        
        private void ShowMessage()
        {
            string fileName = new FileInfo(inputFile).Name;
            var file = new FileInfo(outputFile + $"\\{fileName}");

            string message = $"Файл: {file.Name}";
            message += $"\nРозмір: {file.Length / 1000} КБ";
            MessageBox.Show(message);
        }
        private void Decryption(object sender, DoWorkEventArgs e)
        {
            var cipher = new XOR();

            try
            {
                fileTry = false;
                string fileName = new FileInfo(inputFile).Name;

                string outputPath = $"{outputFile}\\{fileName}";
                string readText = File.ReadAllText(inputFile);

                var decryptedMessage = cipher.Decrypt(readText, key, work);
                File.WriteAllText(outputPath, decryptedMessage);
            }
            catch
            {
                fileTry = true;
                MessageBox.Show("Проблеми з файлом.");
            }
        }
        private void Encryption(object sender, DoWorkEventArgs e)
        {
            var cipher = new XOR();
            
            try
            {
                fileTry = false;
                string fileName = new FileInfo(inputFile).Name;

                string outputPath = $"{outputFile}\\{fileName}";
                string readText = File.ReadAllText(inputFile);

                var encryptedMessage = cipher.Encrypt(readText, key, work);
                File.WriteAllText(outputPath, encryptedMessage);
            }
            catch
            {
                fileTry = true;
                MessageBox.Show("Проблеми з файлом.");
            }
        }
        private void KeyTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string allowedCharacters = "0123456789";
            if (!allowedCharacters.Contains(e.Text[0])) e.Handled = true;
        }
        private void GenerateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int value = rnd.Next(1, 999);
            KeyTextBox.Text = value.ToString();
        }

        
    }
}
