namespace Detector.Models.Base
{
    public interface NodeBase
    {
        ModelBase Model { get; }
        NodeList ChildNodes { get; }        
    }
}
