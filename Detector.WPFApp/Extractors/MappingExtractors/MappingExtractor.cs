using Detector.WPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Extractors.MappingExtractors
{
    public interface MappingExtractor
    {
         IEnumerable<Mapping> Mappings { get; }
    }
}
