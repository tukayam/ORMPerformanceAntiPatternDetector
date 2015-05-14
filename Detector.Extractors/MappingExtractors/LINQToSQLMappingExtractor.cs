using Detector.Models.ORM.Base;
using System;
using System.Collections.Generic;

namespace Detector.Extractors.MappingExtractors
{
    public class LINQToSQLMappingExtractor : MappingExtractor
    {
        public IEnumerable<Mapping> Mappings
        {
            get { throw new NotImplementedException(); }
        }
    }
}
