namespace Detector.Models.ORM
{
    public sealed class DatabaseAccessingForeachLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>, ForEachLoopDeclarationBase where T : ORMToolType
    {
    }
}
