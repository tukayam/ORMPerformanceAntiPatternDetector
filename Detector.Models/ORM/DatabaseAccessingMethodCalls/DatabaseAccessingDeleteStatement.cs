﻿using Detector.Models.Base;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;

namespace Detector.Models.ORM.DatabaseAccessingMethodCalls
{
    public class DatabaseAccessingDeleteStatement<T> : DatabaseAccessingMethodCallStatement<T> where T : ORMToolType
    {
        public DatabaseAccessingDeleteStatement(string queryTextInCSharp
         , HashSet<DatabaseEntityDeclaration<T>> entityDeclarations
         , CompilationInfo compilationInfo)
            : base(queryTextInCSharp, entityDeclarations, compilationInfo)
        { }
    }
}
