using Detector.Models.Base;

namespace Detector.Models.ORM.DatabaseEntities
{
    public class DatabaseEntityDeclaration<T> : DeclarationBase
    {
        public DatabaseEntityDeclaration(string name, CompilationInfo compilationInfo)
            : base(name, compilationInfo)
        { }
    }
}
