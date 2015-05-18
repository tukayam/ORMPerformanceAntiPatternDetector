using Detector.Models.Base;
using Detector.Models.ORM;
using System;

namespace Detector.Models
{
    public abstract class DatabaseAccessingLoopDeclaration<T> : LoopDeclarationBase where T : ORMToolType
    {
        public DatabaseAccessingMethodCallStatement<T> DatabaseAccessingMethodCallStatement { get; }
        public VariableDeclarationInsideDatabaseAccessingLoop<T> VariableDeclaration { get; }

        public CompilationInfo CompilationInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
