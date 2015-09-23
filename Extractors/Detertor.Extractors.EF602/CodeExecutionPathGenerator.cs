using Detector.Extractors.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors.EF602
{
    public class CodeExecutionPathGenerator : CodeExecutionPathGenerator<EntityFramework>
    {
        public CodeExecutionPathGenerator(Context<EntityFramework> context)
            : base(context)
        { }
    }
}
