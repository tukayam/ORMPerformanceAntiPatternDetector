using Detector.Models.ORM;

namespace Detector.Models
{
    public abstract class DatabaseAccessingLoopDeclaration<T> : LoopDeclarationBase where T : ORMToolType
    {
        public DatabaseAccessingMethodCallStatement<T> DatabaseAccessingMethodCallStatement { get; }
        public VariableDeclarationInsideDatabaseAccessingLoop<T> VariableDeclaration { get; }
    }
}
