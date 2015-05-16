namespace Detector.Models.ORM
{
    public class DatabaseEntityObjectUpdateStatement<T> where T : ORMToolType
    {
        public DatabaseEntityObject<T> DatabaseEntityObject { get; private set; }
    }
}
