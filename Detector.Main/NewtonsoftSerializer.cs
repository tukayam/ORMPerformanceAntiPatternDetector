using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Detector.Models.ORM.ORMTools;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Main
{
    public class NewtonsoftSerializer<T> : ISerializer<T> where T : ORMToolType
    {
        public async Task Serialize(HashSet<DatabaseEntityDeclaration<T>> collection, string solutionUnderTest)
        {
            await SerializeBase(collection, solutionUnderTest, "DatabaseEntityDeclarations");
        }

        public async Task Serialize(HashSet<DatabaseAccessingMethodCallStatement<T>> collection, string solutionUnderTest)
        {
            await SerializeBase(collection, solutionUnderTest, "DatabaseAccessingMethodCallStatements");
        }

        public async Task Serialize(HashSet<DatabaseQueryVariable<T>> collection, string solutionUnderTest)
        {
            await SerializeBase(collection, solutionUnderTest, "DatabaseQueryVariables");
        }

        public async Task Serialize(HashSet<DataContextDeclaration<T>> collection, string solutionUnderTest)
        {
            await SerializeBase(collection, solutionUnderTest, "DataContextDeclarations");
        }

        public async Task Serialize(HashSet<CodeExecutionPath> collection, string solutionUnderTest)
        {
            await SerializeBase(collection, solutionUnderTest, "CodeExecutionPaths");
        }

        private async Task SerializeBase(IEnumerable collection, string solutionUnderTest, string fileName)
        {
            using (FileStream fs = File.Open(GetFilePath(solutionUnderTest, fileName), FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.ContractResolver = ShouldSerializeContractResolver.Instance;
                serializer.Serialize(jw, collection);
            }
        }

        private string GetFilePath(string solutionUnderTest, string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string ORMToolTypeSingleName = typeof(T).ToString().Split('.').Last();
            return string.Format(@"{0}\ORMPerformanceAntiPatternDetector\ProjectsUsing{1}\{2}\{3}.json", desktopPath, ORMToolTypeSingleName, solutionUnderTest, fileName);
        }

      
    }
}
