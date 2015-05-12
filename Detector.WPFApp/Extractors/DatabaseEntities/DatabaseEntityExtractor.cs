using Detector.WPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Extractors.DatabaseEntities
{
    public interface DatabaseEntityExtractor<T> where T : Entity
    {
        IEnumerable<T> Entities { get; }
    }
}
