using Detector.Models.Base;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface LoopDeclarationExtractor
    {
        List<LoopDeclarationBase> LoopDeclarations { get; }
    }
}
