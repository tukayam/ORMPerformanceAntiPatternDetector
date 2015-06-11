using System.Collections.Generic;

namespace Detector.Models.Base
{
    public interface NodeBase
    {
        ModelBase Model { get; }
        List<NodeBase> ChildNodes { get; }        
    }
}
