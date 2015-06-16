using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Models.Base
{
    public class NodeList : IEnumerable<NodeBase>
    {
        private List<NodeBase> _nodes;

        public NodeList()
        {
            this._nodes = new List<NodeBase>();
        }

        public void Add(NodeBase node)
        {
            _nodes.Add(node);
        }

        public IEnumerable<NodeBase> OfType<TResult>() where TResult : ModelBase
        {
            foreach (var item in _nodes)
            {
                if (item.Model is TResult)
                {
                    yield return item;
                }
            }
        }

        public IEnumerator<NodeBase> GetEnumerator()
        {
            foreach (var item in _nodes)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public NodeBase this[int index]
        {
            get
            {
                return _nodes[index];
            }
        }
    }
}
