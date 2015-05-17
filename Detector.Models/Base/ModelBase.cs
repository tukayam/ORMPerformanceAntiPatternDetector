using Detector.Models.Compilation;

namespace Detector.Models
{
    public interface ModelBase
    {
        CompilationInfo CompilationInfo { get; }
    }
}
