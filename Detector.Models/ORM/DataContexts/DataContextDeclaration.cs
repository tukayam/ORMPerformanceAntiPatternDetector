using Detector.Models.Base;

namespace Detector.Models.ORM.DataContexts
{
    public class DataContextDeclaration<T> : DeclarationBase
    {
        public DataContextDeclaration(string name, CompilationInfo compilationInfo)
            : base(name, compilationInfo)
        { }
    }
}
