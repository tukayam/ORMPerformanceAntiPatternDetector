using Detector.Models.Base;
using System.Collections.Generic;

namespace Detector.Extractors.Base
{
    public interface LoopDeclarationExtractor : Extractor
    {
        List<LoopDeclarationBase> LoopDeclarations { get; }
    }
}
