using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.Extractors
{
    public class MyAnnotator : CSharpSyntaxRewriter
    {
        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            var newToken = base.VisitToken(token);
            var position = token.ToString().IndexOf('i');
            if (position >= 0)
            {
                newToken = newToken.WithAdditionalAnnotations(MyAnnotation.Create(position));
            }

            return newToken;
        }
    }

    public static class MyAnnotation
    {
        public const string Kind = "MyAnnotation";

        public static SyntaxAnnotation Create(int position)
        {
            return new SyntaxAnnotation(Kind, position.ToString());
        }

        public static int GetPosition(SyntaxAnnotation annotation)
        {
            return int.Parse(annotation.Data);
        }
    }
}
