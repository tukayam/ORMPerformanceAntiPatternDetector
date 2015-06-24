using Detector.Models;
using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Extractors
{
    public interface ExecutionCodePathGeneratorBase<T> where T:ORMToolType
    {
        ORMModelTree GenerateFromModelList(List<ModelBase> models);
    }
}
