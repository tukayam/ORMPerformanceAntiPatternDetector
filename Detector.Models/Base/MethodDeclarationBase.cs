namespace Detector.Models.Base
{
    public interface MethodDeclarationBase : ModelBase
    {
        string MethodName { get; }
        string ContainingClassName { get; }
    }
}
