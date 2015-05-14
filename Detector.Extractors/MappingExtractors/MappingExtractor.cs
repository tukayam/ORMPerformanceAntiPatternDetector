using Detector.Models.ORM.Base;
using System.Collections.Generic;

namespace Detector.Extractors.MappingExtractors
{
    public interface MappingExtractor
    {
         IEnumerable<Mapping> Mappings { get; }
    }
}
