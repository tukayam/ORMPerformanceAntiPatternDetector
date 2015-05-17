namespace Detector.Models.ORM
{
    public class DatabaseAccessingForLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>, ForLoopDeclarationBase where T : ORMToolType
    {
    }
}
