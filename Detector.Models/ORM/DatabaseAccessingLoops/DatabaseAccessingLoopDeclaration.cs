using Detector.Models.Base;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using System;

namespace Detector.Models.ORM.DatabaseAccessingLoops
{
    public abstract class DatabaseAccessingLoopDeclaration<T> : LoopDeclarationBase where T : ORMToolType
    {
        public DatabaseAccessingMethodCallStatement<T> DatabaseAccessingMethodCallStatement { get; }
        public DatabaseEntityVariableDeclaration<T> DatabaseEntityVariableInLooop { get; }

        public CompilationInfo CompilationInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
