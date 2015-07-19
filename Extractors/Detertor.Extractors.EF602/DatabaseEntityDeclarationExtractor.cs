using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.EF602
{
    public class DatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<LINQToSQL>
    {
        public ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> DatabaseEntityDeclarations { get; private set; }

        public async Task<ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>> ExtractAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await document.GetSyntaxRootAsync();

                    var dbEntityDeclarationExtractor = new DatabaseEntityDeclarationExtractorOnOneDocument();
                    dbEntityDeclarationExtractor.Visit(root);
                    foreach (var dbEntityDeclaration in dbEntityDeclarationExtractor.DatabaseEntityDeclarations)
                    {
                        DatabaseEntityDeclarations.Add(dbEntityDeclaration);
                    }
                }
            }

            return DatabaseEntityDeclarations;
        }
    }
}
