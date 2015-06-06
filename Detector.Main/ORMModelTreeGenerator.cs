using System.Collections.Generic;
using Detector.Models;
using Detector.Models.ORM;

namespace Detector.Main
{
    public sealed class ORMModelTreeGenerator<T> where T : ORMToolType
    {
      private  ORMModelNodeGenerator<T> _ORMModelNodeGenerator;

        public ORMModelTreeGenerator()
        {
            _ORMModelNodeGenerator = new ORMModelNodeGenerator<T>();
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

        public ORMModelTree GenerateFromModelList(List<ModelBase> models)
        {
            models.Sort(new ModelBaseComparer());
            //   this.RootNode = models[0];

            foreach (var model in models)
            {
                _ORMModelNodeGenerator.Visit(model);
                // ORMModelNode node = new ORMModelNode(model);
            }

            return null;
        }

        internal class ModelBaseComparer : IComparer<ModelBase>
        {
            public int Compare(ModelBase x, ModelBase y)
            {
                int lineNumberX = x.CompilationInfo.SpanStart;
                int lineNumberY = y.CompilationInfo.SpanStart;
                return lineNumberX < lineNumberY ? 1 : lineNumberX > lineNumberY ? -1 : 0;
            }
        }
    }
}
