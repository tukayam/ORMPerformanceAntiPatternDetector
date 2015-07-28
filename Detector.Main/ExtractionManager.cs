﻿using Detector.Extractors.Base;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using System;
using System.Threading.Tasks;

namespace Detector.Main
{
    public class ExtractionManager<T> : IExtractionManager where T : ORMToolType
    {
        private DataContextDeclarationExtractor<T> _dataContextDeclarationExtractor;
        private DatabaseEntityDeclarationExtractor<T> _databaseEntityDeclarationExtractor;
        private DatabaseAccessingMethodCallExtractor<T> _databaseAccessingMethodCallExtractor;
        IProgress<ExtractionProgress> _progressIndicator;
        private ISerializer<T> _serializer;

        public ExtractionManager(DataContextDeclarationExtractor<T> dataContextDeclarationExtractor
            , DatabaseEntityDeclarationExtractor<T> databaseEntityDeclarationExtractor
            , DatabaseAccessingMethodCallExtractor<T> databaseAccessingMethodCallExtractor
            , IProgress<ExtractionProgress> progressIndicator
            , ISerializer<T> serializer)
        {
            _dataContextDeclarationExtractor = dataContextDeclarationExtractor;
            _databaseEntityDeclarationExtractor = databaseEntityDeclarationExtractor;
            _databaseAccessingMethodCallExtractor = databaseAccessingMethodCallExtractor;
            _progressIndicator = progressIndicator;
            _serializer = serializer;
        }

        public async Task ExtractAllAsync(string solutionUnderTest, string folderPath)
        {
            _progressIndicator.Report(new ExtractionProgress(string.Format("Creating Roslyn solution for {0}.sln...",solutionUnderTest)));
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(string.Format(@"{0}\{1}.sln", folderPath, solutionUnderTest));
           
            await _dataContextDeclarationExtractor.FindDataContextDeclarationsAsync(solution, _progressIndicator);

            _progressIndicator.Report(new ExtractionProgress("Saving Data Context Declarations into json file."));
            await _serializer.Serialize(_dataContextDeclarationExtractor.DataContextDeclarations, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));

            await _databaseEntityDeclarationExtractor.FindDatabaseEntityDeclarationsAsync(solution, _progressIndicator);

            _progressIndicator.Report(new ExtractionProgress("Saving Database Entity Declarations into json file."));
            await _serializer.Serialize(_databaseEntityDeclarationExtractor.DatabaseEntityDeclarations, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));

            await _databaseAccessingMethodCallExtractor.FindDatabaseAccessingMethodCallsAsync(solution, _progressIndicator);
            _progressIndicator.Report(new ExtractionProgress("Saving Database Accessing Method Calls into json file."));
            await _serializer.Serialize(_databaseAccessingMethodCallExtractor.DatabaseAccessingMethodCalls, solutionUnderTest);
            _progressIndicator.Report(new ExtractionProgress("Done"));
        }
    }
}
