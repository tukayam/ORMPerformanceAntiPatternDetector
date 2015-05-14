namespace Detector.Models.ORM.Base
{
    public abstract class DatabaseEntityDeclaration
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
