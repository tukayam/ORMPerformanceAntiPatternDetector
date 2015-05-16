namespace Detector.Models.ORM
{
    public class DatabaseEntityObjectInstantiationStatement<T> where T:ORMToolType
    {
        public DatabaseEntityObject<T> DatabaseEntityObject { get; private set; }
    }
}
