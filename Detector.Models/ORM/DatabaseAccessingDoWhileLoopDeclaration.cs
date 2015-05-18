using Detector.Models.Base;
using System;

namespace Detector.Models.ORM
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
