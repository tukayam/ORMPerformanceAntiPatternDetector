using Detector.Extractors.Base;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Main
{
    public class CodeExecutionPathsConstructor<T> where T : ORMToolType
    {
        private ExtractorFactory<T> _extractorFactory;
        public CodeExecutionPathsConstructor(ExtractorFactory<T> extractorFactory)
        {
            this._extractorFactory = extractorFactory;
        }

        public async Task<IEnumerable<CodeExecutionPath>> Construct(Solution solution)
        {
            DatabaseEntityDeclarationExtractor<T> dbEntityDeclarationExtractor = _extractorFactory.GetDatabaseEntityDeclarationExtractor();
            DatabaseAccessingMethodCallExtractor<T> dbAccessingMethodCallsExtractor = _extractorFactory.GetDatabaseAccessingMethodCallsExtractor();

            //Find all root methods
            //Use call graph construction algorithm to generate code execution paths containing class, method, variable declarations

            //Foreach code execution path, take the documents, send them to extractors, generate a new code execution path with ORM model
            
            return null;
        }
    }
}
