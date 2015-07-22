using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.CodeAnalysis.MSBuild;
using Detector.Models.Base;

namespace Detector.Extractors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _solutionPath = "";
        
        public MainWindow()
        {
            InitializeComponent();
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

     
    }
}
