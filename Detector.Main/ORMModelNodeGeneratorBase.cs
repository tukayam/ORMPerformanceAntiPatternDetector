using Detector.Models;
using Detector.Models.ORM;

namespace Detector.Main
{
    interface ORMModelNodeGeneratorBase<T> where T:ORMToolType
    {
        ORMModelNode Visit(ModelBase model);
        ORMModelNode Visit(DatabaseQuery<T> model);
        ORMModelNode Visit(DatabaseAccessingForeachLoopDeclaration<T> model);
        ORMModelNode Visit(DatabaseAccessingForLoopDeclaration<T> model);
        ORMModelNode Visit(DatabaseAccessingDoWhileLoopDeclaration<T> model);
        ORMModelNode Visit(DatabaseAccessingWhileLoopDeclaration<T> model);
        ORMModelNode Visit(DatabaseAccessingMethodCallStatementOnQueryDeclaration<T> model);
        ORMModelNode Visit(DatabaseAccessingMethodCallStatementOnQueryVariable<T> model);
        ORMModelNode Visit(DatabaseEntityObjectInstantiationStatement<T> model);
        ORMModelNode Visit(DatabaseEntityObjectCallStatement<T> model);
        ORMModelNode Visit(DatabaseEntityObjectRelatedEntitySelectCallStatement<T> model);
        ORMModelNode Visit(DatabaseEntityObjectUpdateStatement<T> model);
        ORMModelNode Visit(DatabaseQueryVariable model);
        ORMModelNode Visit(DataContextInitializationStatement<T> model);
        ORMModelNode Visit(VariableDeclarationInsideDatabaseAccessingLoop<T> model);
    }
}
