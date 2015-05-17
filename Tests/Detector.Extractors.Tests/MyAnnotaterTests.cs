using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class MyAnnotaterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tree = SyntaxFactory.ParseSyntaxTree(@"
using System;
class Program
{
    static void Main()
    {
        int i = 0; Console.WriteLine(i);
    }
}");

            // Tag all tokens that contain the letter 'i'.
            var rewriter = new MyAnnotator();
            SyntaxNode oldRoot = tree.GetRoot();
            SyntaxNode newRoot = rewriter.Visit(oldRoot);

            Assert.IsFalse(oldRoot.ContainsAnnotations);
            Assert.IsTrue(newRoot.ContainsAnnotations);

            // Find all tokens that were tagged with annotations of type MyAnnotation.
            IEnumerable<SyntaxNodeOrToken> annotatedTokens = newRoot.GetAnnotatedNodesAndTokens(MyAnnotation.Kind);
            var results = string.Join("\r\n",
                annotatedTokens.Select(nodeOrToken =>
                {
                    Assert.IsTrue(nodeOrToken.IsToken);
                    var annotation = nodeOrToken.GetAnnotations(MyAnnotation.Kind).Single();
                    return string.Format("{0} (position {1})", nodeOrToken.ToString(), MyAnnotation.GetPosition(annotation));
                }));

            Assert.AreEqual(@"using (position 2)
static (position 4)
void (position 2)
Main (position 2)
int (position 0)
i (position 0)
WriteLine (position 2)
i (position 0)", results);
        }
    }
}
