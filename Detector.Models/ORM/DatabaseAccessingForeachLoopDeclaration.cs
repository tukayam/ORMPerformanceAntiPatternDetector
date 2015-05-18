using Detector.Models.Base;
using System;

namespace Detector.Models.ORM
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
