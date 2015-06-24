using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.MSBuild;
using Detector.Main;
using Detector.Models.AntiPatterns;

namespace Detector.Extractors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string solutionPath = "";
        List<DatabaseEntityDeclaration<LINQToSQL>> entities;
        List<DatabaseAccessingMethodCallStatement<LINQToSQL>> dbAccessingMethods;

        public MainWindow()
        {
            entities = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            dbAccessingMethods = new List<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
            InitializeComponent();
        }

        private void btnChooseSolution_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName.EndsWith(".sln"))
            {
                solutionPath = dialog.FileName;
                lblSolutionName.Content = String.Concat(solutionPath.Substring(0, 8), ".....", solutionPath.Substring(solutionPath.Length - 20, 20));
            }
        }

        private async void btnDetectAntiPatterns_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                MessageBox.Show("Please choose a solution");
                return;
            }

            await DetectAntiPatternsAsync();
        }

        private async Task DetectAntiPatternsAsync()
        {
            var msWorkspace = MSBuildWorkspace.Create();

            //You must install the MSBuild Tools or this line will throw an exception:
            var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;

            //var ORMAntiPatternsDetector = new ORMAntiPatternsDetector<LINQToSQL>();

            //List<AntiPatternBase> detectedAntiPatterns = await ORMAntiPatternsDetector.DetectAsync(solution);
            //foreach (var antiPattern in detectedAntiPatterns)
            //{
            //    Dispatcher.Invoke(new Action(() => lbResults.Items.Add(antiPattern.ToString())));
            //}
        }
    }
}
