using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Models.ORM.DatabaseAccessingLoops
{
    public class DatabaseAccessingWhileLoopDeclaration<T> : DatabaseAccessingLoopDeclaration<T>,WhileLoopDeclarationBase where T : ORMToolType
    {
    }
}
