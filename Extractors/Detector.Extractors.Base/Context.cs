using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public interface Context<T> where T:ORMToolType
    {
        ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
    }
}
