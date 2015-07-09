using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Detector.Models.Others
{
    public class ModelCollection<T>:HashSet<T> where T : ModelBase
    {
        //HashSet<T> _container;

        //public ModelCollection()
        //{
        //    _container = new HashSet<T>();
        //}

        ////public ModelCollection()
        ////{ }

        //public void Add(T obj)
        //{
        //    _container.Add(obj);
        //}        
    }
}
