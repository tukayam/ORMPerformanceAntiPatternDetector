using System;
using Detector.Models.Compilation;

namespace Detector.Models.ORM.Base
{
    public class DatabaseAccessingMethodCallStatement : ModelBase
    {
        public CompilationUnit CompilationUnit
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DatabaseEntityObject DatabaseEntityObject { get; set; }
        public Query Query { get; set; }
    }
}
