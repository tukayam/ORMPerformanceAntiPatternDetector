using Microsoft.Win32;
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

namespace Detector.WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string solution;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnChooseSolution_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName.EndsWith(".sln"))
            {
                solution = dialog.FileName;
                lblSolutionName.Content = String.Concat(solution.Substring(0, 8), ".....", solution.Substring(solution.Length - 20, 20));
            }
        }

        private void btnExtractDatabaseAccessingCode_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<CallGraph> callGraphs = new CallGraphExtractor().Extract(solution);
        }
    }
}
