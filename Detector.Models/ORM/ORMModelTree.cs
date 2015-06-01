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
    }
}
