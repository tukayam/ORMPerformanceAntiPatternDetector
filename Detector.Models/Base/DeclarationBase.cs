namespace Detector.Models.Base
{
    public abstract class DeclarationBase : ModelBase
    {
        public string Name { get; private set; }

        public DeclarationBase(string name, CompilationInfo compilationInfo)
            : base(compilationInfo)
        {
            Name = name;
        }
    }
}
