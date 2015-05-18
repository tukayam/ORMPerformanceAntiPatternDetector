using System.Collections.Generic;

namespace Detector.Models.Base
{
    public interface NodeBase
    {
        List<NodeBase> ChildNodes { get; }
    }
}
