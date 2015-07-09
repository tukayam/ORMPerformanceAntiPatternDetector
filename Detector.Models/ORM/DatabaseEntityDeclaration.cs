using System;
using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseEntityDeclaration<T>:ModelBase where T : ORMToolType
    {
        public string Name
        {
            get;
            private set;
        }

        public CompilationInfo CompilationInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DatabaseEntityDeclaration(string name)
        {
            this.Name = name;
        }
    }
}
