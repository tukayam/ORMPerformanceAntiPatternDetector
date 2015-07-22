using System;
using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseEntityDeclaration<T> : ModelBase where T : ORMToolType
    {
        public string Name { get; private set; }
        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseEntityDeclaration(string name, CompilationInfo compilationInfo)
        {
            this.Name = name;
            this.CompilationInfo = compilationInfo;
        }
    }
}
