using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace ProcessManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            string processName;

            switch (((Button)sender).Name)
            {
                case "NotepadButton":
                    processName = "notepad";
                    break;
                case "CalculatorButton":
                    processName = "calc";
                    break;
                case "FileExplorerButton":
                    processName = "explorer.exe";
                    break;
                case "WordButton":
                    processName = @"C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE";
                    break;
                case "ExcelButton":
                    processName = @"C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE";
                    break;
                default:
                    return;
            }

            ProcessActions.StartProcess(processName);
        }

        private void ReloadDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = ProcessActions.procList;
        }
        private void ChangeProcessPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IdIsCorrect(ProcessIdTextBoxPriority.Text)) return;
            string priority = ProcessPriorityList.Text;
            int processId = Convert.ToInt32(ProcessIdTextBoxPriority.Text);
            bool allowChangeProcessPriority = ProcessActions.AllowActionForProcess(processId);

            if (!allowChangeProcessPriority)
            {
                MessageBox.Show("Даний процес не підлягає видаленню");
                return;
            }

            switch (priority)
            {
                case "Normal":
                    ProcessActions.ChangeProcessPriority(processId, ProcessPriorityClass.Normal);
                    break;
                case "Idle":
                    ProcessActions.ChangeProcessPriority(processId, ProcessPriorityClass.Idle);
                    break;
                case "High":
                    ProcessActions.ChangeProcessPriority(processId, ProcessPriorityClass.High);
                    break;
                case "RealTime":
                    ProcessActions.ChangeProcessPriority(processId, ProcessPriorityClass.RealTime);
                    break;
                default:
                    MessageBox.Show("Оберіть приорітет");
                    return;
            }
        }
        private void KillProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IdIsCorrect(ProcessIdTextBox.Text)) return;
            int processId = Convert.ToInt32(ProcessIdTextBox.Text);
            bool allowProcessKilling = ProcessActions.AllowActionForProcess(processId);

            if (allowProcessKilling) ProcessActions.KillProcess(processId);
            else MessageBox.Show("Даний процес не підлягає видаленню");
        }
        private bool IdIsCorrect(string id)
        {
            if (id == "")
            {
                MessageBox.Show("Некоректне значення id");
                return false;
            }
            return true;
        }
        private void ProcessesListTabFocus(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = ProcessActions.procList;
        }
        private void ProcessesTabFocus(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = null;
        }
        private void KeyTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string allowedCharacters = "0123456789";
            if (!allowedCharacters.Contains(e.Text[0])) e.Handled = true;
        }

        
    }
}
