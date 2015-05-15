using System;
using Detector.Models.Compilation;

namespace Detector.Models.ORM.Base
{
    public abstract class DatabaseEntityObjectCallStatement : ModelBase
    {
        public CompilationUnit CompilationUnit { get; private set; }

        public DatabaseEntityObject DatabaseEntityObject { get; private set; }

        public DatabaseEntityObjectCallStatement(CompilationUnit compilationUnit, DatabaseEntityObject databaseEntityObject)
        {
            this.CompilationUnit = compilationUnit;
            this.DatabaseEntityObject = databaseEntityObject;
        }
    }
}
