using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;

namespace Detector.Extractors.Base.ExtractionStrategies.Parameters
{
    internal class DataContextDeclarationsParameter<T> : ParameterBase<HashSet<DataContextDeclaration<T>>> where T:ORMToolType
    {
        public DataContextDeclarationsParameter(string name, HashSet<DataContextDeclaration<T>> value)
            : base(name, value)
        { }
    }
}
