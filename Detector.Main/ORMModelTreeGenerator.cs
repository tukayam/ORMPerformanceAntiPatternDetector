using System.Collections.Generic;
using Detector.Models;

namespace Detector.Extractors
{
    public sealed class ORMModelTreeGenerator
    {
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

        public ORMModelTree GenerateFromModelList(List<ModelBase> models)
        {
            models.Sort(new ModelBaseComparer());
            //   this.RootNode = models[0];

            foreach (var model in models)
            {

            }

            return null;
        }

        internal class ModelBaseComparer : IComparer<ModelBase>
        {
            public int Compare(ModelBase x, ModelBase y)
            {
                int lineNumberX = x.CompilationInfo.LineNumberStart;
                int lineNumberY = y.CompilationInfo.LineNumberStart;
                return lineNumberX < lineNumberY ? 1 : lineNumberX > lineNumberY ? -1 : 0;
            }
        }
    }
}
