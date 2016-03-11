using Detector.Extractors.Base;
using Detector.Extractors.DatabaseEntities;
using Detector.Main.DetectionRules;
using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Main
{
    public class ExtractionManager<T> : IExtractionManager where T : ORMToolType
    {
        private DataContextDeclarationExtractor<T> _dataContextDeclarationExtractor;
        private DatabaseEntityDeclarationExtractor<T> _databaseEntityDeclarationExtractor;
        private DatabaseAccessingMethodCallExtractor<T> _databaseAccessingMethodCallExtractor;
        private CodeExecutionPathGenerator<T> _codeExecutionPathExtractor;
        IProgress<ExtractionProgress> _progressIndicator;
        private ISerializer<T> _serializer;
        OneByOneProcessingDetectionRule<T> target_one_by_one;
        ExcessiveDataDetectionRule<T> target_excessive_data;

        public ExtractionManager(DataContextDeclarationExtractor<T> dataContextDeclarationExtractor
            , DatabaseEntityDeclarationExtractor<T> databaseEntityDeclarationExtractor
            , DatabaseAccessingMethodCallExtractor<T> databaseAccessingMethodCallExtractor
            , CodeExecutionPathGenerator<T> codeExecutionPathExtractor
            , IProgress<ExtractionProgress> progressIndicator
            , ISerializer<T> serializer)
        {
            _dataContextDeclarationExtractor = dataContextDeclarationExtractor;
            _databaseEntityDeclarationExtractor = databaseEntityDeclarationExtractor;
            _databaseAccessingMethodCallExtractor = databaseAccessingMethodCallExtractor;
            _codeExecutionPathExtractor = codeExecutionPathExtractor;
            _progressIndicator = progressIndicator;
            _serializer = serializer;
            target_one_by_one = new OneByOneProcessingDetectionRule<T>();
            target_excessive_data = new ExcessiveDataDetectionRule<T>();
        }
        public HashSet<CodeExecutionPath> CodeExecutionPaths_one_by_one { get; private set; }
        public HashSet<CodeExecutionPath> CodeExecutionPaths_excessive_data { get; private set; }

        public async Task ExtractAllAsync(string solutionUnderTest, string folderPath)
        {
            _progressIndicator.Report(new ExtractionProgress(string.Format("Creating Roslyn solution for {0}.sln...", solutionUnderTest)));
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(string.Format(@"{0}\{1}.sln", folderPath, solutionUnderTest));

            await ExtractDataContexts(solutionUnderTest, solution);
            await ExtractDatabaseEntities(solutionUnderTest, solution);
            await ExtractDatabaseAccessingMethodCalls(solutionUnderTest, solution);
            await GenerateCodeExecutionPaths(solutionUnderTest, solution);
            DetectAntiPatterns(solutionUnderTest, solution);
        }

        private void DetectAntiPatterns(string solutionUnderTest, Solution solution)
        {
            CodeExecutionPaths_one_by_one = new HashSet<CodeExecutionPath>();
            CodeExecutionPaths_excessive_data = new HashSet<CodeExecutionPath>();
            // poorly coded for now, will fix this later
            // check one Code Execution Path at a time? guessing this is how to call it
            foreach (var path in _codeExecutionPathExtractor.CodeExecutionPaths)
            {
                bool result_one_by_one = target_one_by_one.AppliesToModelTree(path);
                bool result_excessive_data = target_excessive_data.AppliesToModelTree(path);
                // if one by one is detected, add it to the list of one by one
                if (result_one_by_one)
                {
                    CodeExecutionPaths_one_by_one.Add(path);
                }
                // if excessive data is detected, add it to the list of excessive data
                if (result_excessive_data)
                {
                    CodeExecutionPaths_excessive_data.Add(path);
                }
            }
            // print the counted antipatterns
            Console.WriteLine("Counted {0} one by one processing antipatterns", CodeExecutionPaths_one_by_one.Count);
            Console.WriteLine("Counted {0} excesive data antipatterns", CodeExecutionPaths_excessive_data.Count);
        }


        private async Task GenerateCodeExecutionPaths(string solutionUnderTest, Solution solution)
        {
            await _codeExecutionPathExtractor.GenerateCodeExecutionPathsAsync(solution, _progressIndicator);
            _progressIndicator.Report(new ExtractionProgress("Saving Code Execution Paths into json file."));
            await _serializer.Serialize(_codeExecutionPathExtractor.CodeExecutionPaths, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));
        }

        private async Task ExtractDatabaseAccessingMethodCalls(string solutionUnderTest, Solution solution)
        {
            await _databaseAccessingMethodCallExtractor.FindDatabaseAccessingMethodCallsAsync(solution, _progressIndicator);
            _progressIndicator.Report(new ExtractionProgress("Saving Database Accessing Method Calls into json file."));
            await _serializer.Serialize(_databaseAccessingMethodCallExtractor.DatabaseAccessingMethodCalls, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));
        }

        private async Task ExtractDatabaseEntities(string solutionUnderTest, Solution solution)
        {
            await _databaseEntityDeclarationExtractor.FindDatabaseEntityDeclarationsAsync(solution, _progressIndicator);

            _progressIndicator.Report(new ExtractionProgress("Saving Database Entity Declarations into json file."));
            await _serializer.Serialize(_databaseEntityDeclarationExtractor.DatabaseEntityDeclarations, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));
        }

        private async Task ExtractDataContexts(string solutionUnderTest, Solution solution)
        {
            await _dataContextDeclarationExtractor.FindDataContextDeclarationsAsync(solution, _progressIndicator);
            _progressIndicator.Report(new ExtractionProgress("Saving Data Context Declarations into json file."));
            await _serializer.Serialize(_dataContextDeclarationExtractor.DataContextDeclarations, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));
        }
    }
}
