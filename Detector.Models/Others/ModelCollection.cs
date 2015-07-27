using System.Collections.Generic;

namespace Detector.Models.Others
{
    public class ModelCollection<T> : HashSet<T> where T : ModelBase
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
