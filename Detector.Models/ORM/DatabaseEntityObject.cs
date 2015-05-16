namespace Detector.Models.ORM
{
    public class DatabaseEntityObject<T> where T:ORMToolType
    {
        public DatabaseEntityDeclaration<T> DatabaseEntityDeclaration { get; private set; }
    }
}
