using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.Extractors.Extractors.CallGraphExtractors
{
    public class CallGraphExtractor : CSharpSyntaxWalker
    {
        public override void VisitMethodDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax node)
        {            
            base.VisitMethodDeclaration(node);
        }

        public override void VisitAssignmentExpression(Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax node)
        {
            base.VisitAssignmentExpression(node);
        }

        public override void VisitExpressionStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax node)
        {
            base.VisitExpressionStatement(node);
        }

        public override void VisitForEachStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax node)
        {
            base.VisitForEachStatement(node);
        }

        public override void VisitForStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax node)
        {
            base.VisitForStatement(node);
        }

        public override void VisitObjectCreationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ObjectCreationExpressionSyntax node)
        {
            base.VisitObjectCreationExpression(node);
        }

        public override void VisitQueryBody(Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax node)
        {
            base.VisitQueryBody(node);
        }

        public override void VisitQueryExpression(Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax node)
        {
            base.VisitQueryExpression(node);
        }

        public override void VisitVariableDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax node)
        {
            base.VisitVariableDeclaration(node);
        }


        public override void DefaultVisit(Microsoft.CodeAnalysis.SyntaxNode node)
        {

            base.DefaultVisit(node);
        }
    }
}
