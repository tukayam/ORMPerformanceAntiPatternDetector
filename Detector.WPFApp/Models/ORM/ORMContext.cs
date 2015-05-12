using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Models.ORM
{
    public static class ORMContext
    {
        public static IEnumerable<Query> Queries;
        public static IEnumerable<Entity> Entities;
    }
}
