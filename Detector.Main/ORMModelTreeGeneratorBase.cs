using Detector.Models;
using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Main
{
    interface ORMModelTreeGeneratorBase<T> where T:ORMToolType
    {
        ORMModelTree GenerateFromModelList(List<ModelBase> models);
    }
}
