using Detector.Models.Base;
using System.Collections.Generic;

namespace Detector.Models.ORM
{
    public class ORMModelNode : Node
    {
        public List<NodeBase> ChildNodes { get; private set; }
        public ModelBase Model { get; private set; }

        public ORMModelNode(ModelBase model)
        {
            this.Model = model;
            this.ChildNodes = new List<NodeBase>();
        }

        public void SetChildNodes(List<NodeBase> childNodes)
        {
            this.ChildNodes = childNodes;
        }
    }
}
