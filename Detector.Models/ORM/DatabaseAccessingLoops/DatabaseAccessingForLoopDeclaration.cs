using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;
using System;

namespace Detector.Models.ORM.DatabaseAccessingLoops
{
    public class DatabaseAccessingForLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>, ForLoopDeclarationBase where T : ORMToolType
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
