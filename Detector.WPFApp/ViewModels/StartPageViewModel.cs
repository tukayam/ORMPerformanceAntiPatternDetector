using Detector.Models.ORM;
using Detector.Models.ORM.ORMTools;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using System.Windows.Input;

namespace Detector.WPFApp.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private string _solutionPath = string.Empty;
        public string SolutionPath
        {
            get { return _solutionPath; }
            set
            {
                SetProperty(ref this._solutionPath, value);
            }
        }

        private ORMToolType _ORMToolType;

        public ICommand ChooseSolutionCommand { get; private set; }
        public ICommand SetORMToolToEFCommand { get; private set; }
        public ICommand SetORMToolToNHCommand { get; private set; }
        public ICommand SetORMToolToL2SCommand { get; private set; }

        public StartPageViewModel()
        {
            this.ChooseSolutionCommand = new DelegateCommand(this.OnChooseSolution);
            //this.SetORMToolToEFCommand = new DelegateCommand<ORMToolType>(this.OnSetORMToolCommand());
            //this.SetORMToolToNHCommand = new DelegateCommand(this.OnSetORMToolCommand(new NHibernate()));
            //this.SetORMToolToL2SCommand = new DelegateCommand(this.OnSetORMToolCommand(new LINQToSQL()));
        }

        private void OnChooseSolution()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName.EndsWith(".sln"))
            {
                SolutionPath = dialog.FileName;
            }
        }

        private void OnSetORMToolCommand(ORMToolType ormToolType)
        {
            this._ORMToolType = ormToolType;
        }
    }
}
