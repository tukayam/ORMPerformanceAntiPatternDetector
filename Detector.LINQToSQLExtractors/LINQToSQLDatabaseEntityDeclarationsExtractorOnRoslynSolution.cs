using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.LINQToSQLExtractors
{
    public class LINQToSQLDatabaseEntityDeclarationsExtractorOnRoslynSolution
    {
        public async Task<List<DatabaseEntityDeclaration<LINQToSQL>>> ExtractFromSolution(Solution solution)
        {
            var dbEntityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>();

            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await document.GetSyntaxRootAsync();

                    //Act
                    var dbEntityDeclarationExtractor = new LINQToSQLDatabaseEntityDeclarationExtractor();
                    dbEntityDeclarationExtractor.Visit(root);
                    var result = dbEntityDeclarationExtractor.DatabaseEntityDeclarations;
                    if (result.Count > 0)
                    {
                        dbEntityDeclarations.AddRange(result);
                    }
                }
            }

            return dbEntityDeclarations;
        }
    }
}
