using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Models
{
    public abstract class Entity
    {
        public string Name
        {
            get;
            private set;
        }

        public Entity(string name)
        {
            this.Name = name;
        }
    }
}
