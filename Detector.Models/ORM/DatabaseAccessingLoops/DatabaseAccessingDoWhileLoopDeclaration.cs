using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;
using System;

namespace Detector.Models.ORM.DatabaseAccessingLoops
{
    public class DatabaseAccessingDoWhileLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>, DoWhileLoopDeclarationBase where T : ORMToolType
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
