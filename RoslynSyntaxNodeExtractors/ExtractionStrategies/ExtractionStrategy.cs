using Detector.Extractors.Base.ExtractionStrategies.ReturnTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Extractors.Base.ExtractionStrategies
{
    public interface ExtractionStrategy
    { }

    internal abstract class ExtractionStrategy<T> : ExtractionStrategy where T : ExtractionReturnTypeBase
    {       
        internal ExtractionStrategy(params Parameter[] parameters)
        {
            SetParameters(parameters);
        }

        internal abstract void SetParameters(params Parameter[] parameters);
        internal abstract Task<HashSet<T>> Execute();
    }
}
