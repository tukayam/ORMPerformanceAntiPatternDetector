using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public interface Context<T> where T : ORMToolType
    {
        ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        ModelCollection<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; set; }
        ModelCollection<DatabaseQuery<T>> DatabaseQueries { get; set; }
        ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
    }
}
