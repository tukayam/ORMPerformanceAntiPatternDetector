using Detector.Models.ORM.Base;
using System.Collections.Generic;

namespace Detector.Models.ORM
{
    public static class ORMContext
    {
        public static IEnumerable<Query> Queries;
        public static IEnumerable<DatabaseEntityDeclaration> Entities;
    }
}
