using Detector.Models.ORM;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Detector.WPFApp.Views
{
    /// <summary>
    /// Interaction logic for Page_Start.xaml
    /// </summary>
    public partial class Page_Start : Page
    {
        string _solutionPath = "";
        ORMToolType _ORMToolType;

        public Page_Start()
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
            }
        }

        private void rdBtnEF_Checked(object sender, RoutedEventArgs e)
        {
            _ORMToolType = new EntityFramework();
        }

        private void rdBtnL2S_Checked(object sender, RoutedEventArgs e)
        {
            _ORMToolType = new LINQToSQL();
        }

        private void rdBtnNHibernate_Checked(object sender, RoutedEventArgs e)
        {
            _ORMToolType = new NHibernate();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //Check whether solution is set and a tool is chosen

            //Redirect to next page
        }       
    }
}
