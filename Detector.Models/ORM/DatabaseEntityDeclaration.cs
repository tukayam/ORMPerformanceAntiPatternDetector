namespace Detector.Models.ORM
{
    public class DatabaseEntityDeclaration<T> where T : ORMToolType
    {
        public string Name
        {
            get;
            private set;
        }

        public DatabaseEntityDeclaration(string name)
        {
            this.Name = name;
        }
    }
}
