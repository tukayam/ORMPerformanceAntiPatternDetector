using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Detector.Extractors.EF602
{
    public sealed class DatabaseEntityDeclarationExtractorOnOneDocument : CSharpSyntaxWalker
    {
        private List<DatabaseEntityDeclaration<EntityFramework>> _entities;

        public List<DatabaseEntityDeclaration<EntityFramework>> DatabaseEntityDeclarations
        {
            get
            {
                return _entities;
            }
        }

        public DatabaseEntityDeclarationExtractorOnOneDocument()
            : base()
        {
            _entities = new List<DatabaseEntityDeclaration<EntityFramework>>();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
           

            base.VisitClassDeclaration(node);
        }
    }
}
