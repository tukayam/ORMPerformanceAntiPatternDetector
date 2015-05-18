using Detector.Models.Base;
using System.Collections.Generic;
using Detector.Models;
using System;

namespace Detector.Main
{
    public class RoslynORMSyntaxTreeListExtractor
    {
        public List<ORMModelTree> ORMSyntaxTreeList { get; private set; }
        private SyntaxTreeWalker syntaxTreeWalker;

        public RoslynORMSyntaxTreeListExtractor(SyntaxTreeWalker syntaxTreeWalker)
        {
            this.syntaxTreeWalker = syntaxTreeWalker;
        }

        // Change to implement with CompilationUnit from Roslyn
        //public IEnumerable<ORMSyntaxTree> Extract(IEnumerable<CompilationUnit> compilationUnitList)
        //{

        //    ORMSyntaxTreeList = new List<ORMSyntaxTree>();

        //    foreach (var compilationUnit in compilationUnitList)
        //    {
        //        syntaxTreeWalker.Visit(compilationUnit.SyntaxTree.GetRoot());
        //        ORMSyntaxTreeList.Add(syntaxTreeWalker.ORMSyntaxTree);
        //    }

        //    return ORMSyntaxTreeList;
        //}
    }
}
