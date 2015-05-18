using System.Collections.Generic;
using Detector.Models;

namespace Detector.Extractors
{
    public sealed class LINQToSQLORMModelTreeGenerator
    {
        public LINQToSQLORMModelTreeGenerator()
        {
        }

        public ORMModelTree GetORMModelTree()
        {
            var allModels = new List<ModelBase>();
            //allModels.AddRange(MethodDeclarations);
            //allModels.AddRange(DatabaseAccessingMethodCalls);
            //allModels.AddRange(DatabaseAccessingForeachLoopDeclarations);
            //allModels.AddRange(DatabaseAccessingForLoopDeclarations);
            //allModels.AddRange(ForeachLoopDeclarations);
            //allModels.AddRange(ForLoopDeclarations);
            //allModels.AddRange(WhileLoopDeclarations);


            //  ORMModelTree ORMModelTree = new ORMModelTree();
            return null;
        }
    }
}
