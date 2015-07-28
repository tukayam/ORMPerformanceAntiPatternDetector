using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;
using System;

namespace Detector.Models.ORM.DatabaseAccessingLoops
{
    public sealed class DatabaseAccessingForeachLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>, ForEachLoopDeclarationBase where T : ORMToolType
    {
        public CompilationInfo CompilationInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
