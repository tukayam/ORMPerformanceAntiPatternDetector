using Detector.Models.ORM;

namespace Detector.Models
{
    public abstract class DatabaseAccessingLoopDeclaration<T> where T : ORMToolType
    {
        public DatabaseAccessingMethodCallStatement<T> DatabaseAccessingMethodCallStatement { get; }
        public VariableDeclarationInsideDatabaseAccessingLoop<T> VariableDeclaration { get; }
    }
}
