using System;
using System.Collections.Generic;
using Detector.Models.Base;

namespace Detector.Models.Others
{
    public class MethodDeclaration : Node, MethodDeclarationBase
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public string MethodName { get; private set; }

        public List<NodeBase> ChildNodes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MethodDeclaration(string methodName, CompilationInfo compilationInfo)
        {
            this.MethodName = methodName;
            this.CompilationInfo = compilationInfo;
        }
    }
}
