using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DatabaseEntities
{
    public class DatabaseEntityObjectUpdateStatement<T> where T : ORMToolType
    {
        public DatabaseEntityVariableDeclaration<T> DatabaseEntityObject { get; private set; }
    }
}
