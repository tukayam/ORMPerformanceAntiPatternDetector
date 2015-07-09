using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.MSBuild;
using Detector.Main;
using Detector.Models.AntiPatterns;
using Detector.Models.Base;
using Detector.Models.Others;

namespace Detector.Extractors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _solutionPath = "";

        HashSet<Document> _documentsInSolution;


        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnChooseSolution_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName.EndsWith(".sln"))
            {
                _solutionPath = dialog.FileName;
                lblSolutionName.Content = String.Concat(_solutionPath.Substring(0, 8), ".....", _solutionPath.Substring(_solutionPath.Length - 20, 20));

                lblSandboxExtractingDocumentsInSolution.Visibility = Visibility.Visible;
                _documentsInSolution = await GetDocumentsInSolutionAsync();
                btnChooseDocuments.Visibility = Visibility.Visible;
            }
        }

        private void btnChooseDocuments_Click(object sender, RoutedEventArgs e)
        {
           // 
        }

        private async Task<HashSet<Document>> GetDocumentsInSolutionAsync()
        {
            return new HashSet<Document>();
        }

        private async void btnDetectAntiPatterns_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_solutionPath))
            {
                MessageBox.Show("Please choose a solution");
                return;
            }

            HashSet<CodeExecutionPath> codeExecutionPaths = await GetCodeExecutionPathsAsync();

            await DetectAntiPatternsOnCodeExecutionPathsAsync(codeExecutionPaths);
        }

        private async Task<HashSet<CodeExecutionPath>> GetCodeExecutionPathsAsync()
        {
            return new HashSet<CodeExecutionPath>();
        }

        private async Task DetectAntiPatternsOnCodeExecutionPathsAsync(HashSet<CodeExecutionPath> codeExecutionPaths)
        {
            var msWorkspace = MSBuildWorkspace.Create();

            //You must install the MSBuild Tools or this line will throw an exception:
            var solution = msWorkspace.OpenSolutionAsync(_solutionPath).Result;

            //var ORMAntiPatternsDetector = new ORMAntiPatternsDetector<LINQToSQL>();

            //List<AntiPatternBase> detectedAntiPatterns = await ORMAntiPatternsDetector.DetectAsync(solution);
            //foreach (var antiPattern in detectedAntiPatterns)
            //{
            //    Dispatcher.Invoke(new Action(() => lbResults.Items.Add(antiPattern.ToString())));
            //}
        }

        private void btnChooseORMTool_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
