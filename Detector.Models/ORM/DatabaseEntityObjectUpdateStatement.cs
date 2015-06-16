namespace Detector.Models.ORM
{
    public class DatabaseEntityObjectUpdateStatement<T> where T : ORMToolType
    {
        public DatabaseEntityVariable<T> DatabaseEntityObject { get; private set; }
    }
}
