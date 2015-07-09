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
            DatabaseAccessingMethodCallsExtractor<T> dbAccessingMethodCallsExtractor=  _extractorFactory.GetDatabaseAccessingMethodCallsExtractor();


            //Go through each file in the solution
            //Call the db accessing method call extractor to get all the method calls
            //Get CompilationInfo of each method call to get method declaration
            //Create a code execution path for each method declaration
            //Find all calls to the method declaration
            //Create a new code execution path

            //Fill in each method declaration in code execution path with new models

            return null;
                }
    }
}
