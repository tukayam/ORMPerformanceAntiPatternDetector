using System;
using System.Collections.Generic;
using Detector.Models.Base;

namespace Detector.Models
{
    public class ORMModelTree 
    {
        public NodeBase RootNode
        {
            get;
            private set;
        }

        public ORMModelTree()
        {
        }

        public ORMModelTree(NodeBase rootNode)
            : this()
        {
            this.RootNode = rootNode;
        }        
    }
}
