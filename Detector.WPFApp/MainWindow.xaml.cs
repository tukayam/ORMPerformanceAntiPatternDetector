using Detector.WPFApp.Extractors.DatabaseEntities;
using Detector.WPFApp.Models.ORM.LINQToSQL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
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

                    var root = await Task.Run(() => document.GetSyntaxRootAsync());

                    LINQToSQLDatabaseEntityExtractor LINQToSQLDatabaseEntityExtractor = new LINQToSQLDatabaseEntityExtractor();
                    LINQToSQLDatabaseEntityExtractor.Visit(root);

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
