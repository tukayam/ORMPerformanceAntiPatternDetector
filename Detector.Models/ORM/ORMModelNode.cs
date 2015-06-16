using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class ORMModelNode : Node
    {
        public NodeList ChildNodes { get; private set; }
        public ModelBase Model { get; private set; }

        public ORMModelNode(ModelBase model)
        {
            this.Model = model;
            this.ChildNodes = new NodeList();
        }

        public void SetChildNodes(NodeList childNodes)
        {
            this.ChildNodes = childNodes;
        }
    }
}
