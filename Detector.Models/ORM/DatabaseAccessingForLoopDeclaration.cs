using Detector.Models.Base;
using System;

namespace Detector.Models.ORM
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
