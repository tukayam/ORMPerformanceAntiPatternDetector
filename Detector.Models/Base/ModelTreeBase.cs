using System.Collections;
using System.Collections.Generic;

namespace Detector.Models.Base
{
    public interface ModelTreeBase
    {
        NodeBase RootNode { get; }

        IEnumerable<NodeBase> OfType<ModelBase>();
    }
}
