using Detector.Models.Base;
using System;
using System.Collections.Generic;

namespace Detector.Models.ORM
{
    public abstract class ORMModelNode<T> : Node<T> where T : ModelBase
    {
        public IEnumerable<NodeBase> ChildNodes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T Model
        {
            get
            {
                throw new NotImplementedException();
            }
        }


    }
}
