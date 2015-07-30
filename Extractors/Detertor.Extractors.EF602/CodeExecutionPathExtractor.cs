using Detector.Extractors.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors.EF602
{
    public class CodeExecutionPathExtractor : CodeExecutionPathExtractor<EntityFramework>
    {
        public CodeExecutionPathExtractor(Context<EntityFramework> context)
            : base(context)
        { }
    }
}
