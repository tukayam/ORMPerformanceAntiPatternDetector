using Detector.Models.Compilation;
using Detector.Models.ORM.Base;

namespace Detector.Models
{
    public interface ModelBase
    {
        CompilationUnit CompilationUnit { get; }
    }
}
