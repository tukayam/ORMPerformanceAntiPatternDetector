using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.Models
{
    public class ORMSyntaxTree
    {
        public List<ModelBase> Nodes { get; set; }

        public ORMSyntaxTree()
        {
            Nodes = new List<ModelBase>();
        }
    }
}
