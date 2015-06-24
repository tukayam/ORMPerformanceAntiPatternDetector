namespace Detector.Models.ORM
{
    public class DatabaseEntityObjectUpdateStatement<T> where T : ORMToolType
    {
        public DatabaseEntityVariableDeclaration<T> DatabaseEntityObject { get; private set; }
    }
}
