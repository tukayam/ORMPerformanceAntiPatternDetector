using Detector.WPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Extractors.MappingExtractors
{
    public class LINQToSQLMappingExtractor : MappingExtractor
    {
        public IEnumerable<Mapping> Mappings
        {
            get { throw new NotImplementedException(); }
        }
    }
}
