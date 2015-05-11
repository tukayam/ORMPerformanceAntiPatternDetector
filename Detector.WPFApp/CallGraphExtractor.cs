using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp
{
    class CallGraphExtractor
    {
        public IEnumerable<CallGraph> Extract()
        {
            return null;
        }

        //internal IEnumerable<CallGraph> Extract(string solution)
        //{
//            // 0) Creates a NDependServicesProvider object
//var ndependServicesProvider = new NDependServicesProvider();
 
 
//// 1) obtain some VS solution or project file path
//var visualStudioManager = ndependServicesProvider.VisualStudioManager;
//ICollection<IAbsoluteFilePath> vsSlnOrProjFilePaths;
//IntPtr ownerWindowHandle = ...; // get a non IntPtr.Zero owner Window Handle. If you are in a System.Console try this
//visualStudioManager.ShowDialogSelectVisualStudioSolutionsOrProjects(ownerWindowHandle, out vsSlnOrProjFilePaths);
//// Could also use:  visualStudioManager.GetMostRecentlyUsedVisualStudioSolutionOrProject() can also be used
 
 
//// 2) obtains assemblies file path to analyze
//var assembliesFilePath = (from vsSlnOrProjFilePath in vsSlnOrProjFilePaths
//                           from assembliesFilePathTmp in visualStudioManager.GetAssembliesFromVisualStudioSolutionOrProject(vsSlnOrProjFilePath)
//                           select assembliesFilePathTmp).Distinct().ToArray();
 
//// 3) gets or create a IProject object
//var projectManager = ndependServicesProvider.ProjectManager;
//IProject project = projectManager.CreateTemporaryProject(assembliesFilePath, TemporaryProjectMode.Temporary);
//// Or, to get a IProject object, could also use
////   projectManager.CreateBlankProject()  to create a persisten project
////   and then project.CodeToAnalyze.SetApplicationAssemblies()
////   and then projectManager.SaveProject(project); o save the project file
////
//// Or, to get an existing IProject object, could also use
////    projectManager.ShowDialogChooseAnExistingProject(out project)
//// Or programmatically list most recently used NDepend projects on this machine through
////    projectManager.GetMostRecentlyUsedProjects()
 
 
//// 4) gets an IAnalysisResult object from the IProject object
//IAnalysisResult analysisResult = project.RunAnalysis();  // *** This particular method works only with a Build Machine license ***
//                                             //  Or  project.RunAnalysisAndBuildReport()  // *** This particular method works only with a Build Machine license ***
 
//// Or, to get a IAnalysisResult object, first gets a IAnalysisResultRef object, that represents a reference to a persisted IAnalysisResult object
////    project.TryGetMostRecentAnalysisResultRef() or project.GetAvailableAnalysisResultsRefs() or project.GetAvailableAnalysisResultsRefsGroupedPerMonth()
//// and then analysisResultRef.Load()
 
 
//// 5) gets a ICodeBase object from the IAnalysisResult object
//ICodeBase codeBase = analysisResult.CodeBase;
//// Or eventually a ICompareContext object if you wish to analyze diff
//// codeBase.CreateCompareContextWithOlder(olderCodeBase)
 
 
//// 6) use the code model API to query code and do develop any algorithm you need!
//// For example here we are looking for complex methods
//var complexMethods = (from m in codeBase.Application.Methods
//                        where m.ILCyclomaticComplexity > 10
//                        orderby m.ILCyclomaticComplexity descending
//                        select m).ToArray();
//if (complexMethods.Length == 0) { return; }
//Console.WriteLine("Press a key to show the " + complexMethods.Length + " most complex methods");
//Console.ReadKey();
//foreach (var m in complexMethods) {
//   Console.WriteLine(m.FullName + " has a IL cyclomatic complexity of " + m.ILCyclomaticComplexity);
//}
 
 
//// 7) eventually lets the user opens source file declaration
//if (complexMethods.First().SourceFileDeclAvailable) {
//   var mostComplexMethod = complexMethods.First();
//   Console.WriteLine("Press a key to open the source code decl of the most complex method?");
//   Console.ReadKey();
//   mostComplexMethod.TryOpenSource();
//   // Eventually use ExtensionMethodsTooling.TryCompareSourceWith(NDepend.CodeModel.ISourceFileLine,NDepend.CodeModel.ISourceFileLine)
//   // to compare 2 different versions of a code element
//}
        //}
    }
}
