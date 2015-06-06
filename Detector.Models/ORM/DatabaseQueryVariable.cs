using System;
using Detector.Models.Base;

namespace Detector.Models.ORM
{
    public class DatabaseQueryVariable : ModelBase
    {
        public string VariableName { get; private set; }

        public CompilationInfo CompilationInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DatabaseQueryVariable(string variableName)
        {
            this.VariableName = variableName;
        }
    }
}
