using Detector.Extractors.Base;
using Detector.Extractors.EF602;
using Detector.Main;
using Detector.Models.ORM.ORMTools;
using System;

namespace Detector.ConsoleApp
{
    public class DependencyResolver
    {
        public IExtractionManager GetExtractionManager()
        {
            var context = new ConcreteContext<EntityFramework>();
            var dataContextDecExt = new DataContextDeclarationExtractor(context);
            var dbEntityExt = new DatabaseEntityDeclarationExtractorUsingDbContextProperties(context);
            var dbAccessingMethodCallExt = new DatabaseAccessingMethodCallExtractor(context);
            var codeExecutionPathExt = new CodeExecutionPathExtractor(context);
            var progressIndicator = new Progress<ExtractionProgress>((e) => ProgressChanged(e));
            var serializer = new NewtonsoftSerializer<EntityFramework>();

            IExtractionManager extractionManager = new ExtractionManager<EntityFramework>(dataContextDecExt, dbEntityExt, dbAccessingMethodCallExt, codeExecutionPathExt, progressIndicator, serializer);
            return extractionManager;
        }

        private void ProgressChanged(ExtractionProgress extractionProgress)
        {
            if (!string.IsNullOrEmpty(extractionProgress.ExtractionType))
            {
                Console.WriteLine();
                Console.WriteLine(extractionProgress.ExtractionType);                
            }
            else
            {
                Console.Write("    \r{0}%  done ", extractionProgress.PercentageOfWorkDone);
            }
        }
    }
}
