using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingWhileLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>,WhileLoopDeclarationBase where T : ORMToolType
    {
    }
}
