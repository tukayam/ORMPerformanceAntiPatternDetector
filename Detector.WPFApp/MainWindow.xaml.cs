using Detector.Extractors.Extractors.CallGraphExtractors;
using Detector.Models.ORM.LINQToSQL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Detector.Extractors.DatabaseEntities;

namespace Detector.Extractors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string solutionPath = "";
        List<LINQToSQLEntity> entities;
        public MainWindow()
        {
            entities = new List<LINQToSQLEntity>();
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

        private void btnExtractDatabaseAccessingCode_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                MessageBox.Show("Please choose a solution");
                return;
            }
            Task task = new Task(ExtractEntities);
            task.Start();
            task.Wait();
        }

        async void ExtractEntities()
        {
            var msWorkspace = MSBuildWorkspace.Create();
            //You must install the MSBuild Tools or this line will throw an exception:
            var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await Task.Run(() => document.GetSyntaxRootAsync());

                    LINQToSQLDatabaseEntityExtractor LINQToSQLDatabaseEntityExtractor = new LINQToSQLDatabaseEntityExtractor();
                    LINQToSQLDatabaseEntityExtractor.Visit(root);


                    CallGraphExtractor ext = new CallGraphExtractor();
                    ext.Visit(root);

                    SyntaxTree tree = await Task.Run(() => document.GetSyntaxTreeAsync());


                    var compilation = CSharpCompilation.Create("HelloWorld")
                        .AddReferences(MetadataReference.CreateFromAssembly(typeof(object).Assembly))
                        .AddSyntaxTrees(new SyntaxTree[] { tree });
                    var model = compilation.GetSemanticModel(tree);


                    entities.AddRange(LINQToSQLDatabaseEntityExtractor.Entities);
                }
            }

            foreach (var entity in entities)
            {
                Dispatcher.Invoke(new Action(() => lbResults.Items.Add(entity.Name)));
            }
        }
    }
}
