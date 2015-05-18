using System;
using System.Collections.Generic;
using Detector.Models.Base;

namespace Detector.Models
{
    public class ORMModelTree : ModelTreeBase
    {
        public NodeBase RootNode
        {
            get;
            private set;
        }

        public ORMModelTree(NodeBase rootNode)
        {
            this.RootNode = rootNode;
        }

        public IEnumerable<NodeBase> OfType<ModelBase>()
        {
            throw new NotImplementedException();
        }

        public void GenerateFromModelList(List<ModelBase> models)
        {
            models.Sort(new ModelBaseComparer());
            //   this.RootNode = models[0];

            foreach (var model in models)
            {

            }
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
