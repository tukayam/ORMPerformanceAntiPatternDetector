using Detector.Models.ORM;
using Detector.Models.Others;
using System.Threading.Tasks;

namespace Detector.Main
{
    public interface ISerializer<T> where T : ORMToolType
    {
        Task Serialize(ModelCollection<DataContextDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(ModelCollection<DatabaseEntityDeclaration<T>> collection, string solutionUnderTest);
        Task Serialize(ModelCollection<DatabaseQueryVariable<T>> collection, string solutionUnderTest);
        Task Serialize(ModelCollection<DatabaseAccessingMethodCallStatement<T>> collection, string solutionUnderTest);
    }
}
