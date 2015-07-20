using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Models.Others;
using Detector.Extractors.Base;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<LINQToSQL>
    {
        public override ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> DatabaseEntityDeclarations { get; }

        public LINQToSQLDatabaseEntityDeclarationExtractor(Context<LINQToSQL> context)
            : base(context)
        { }

        public override async Task FindDatabaseEntityDeclarationsAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await document.GetSyntaxRootAsync();

                    var dbEntityDeclarationExtractor = new LINQToSQLDatabaseEntityDeclarationExtractorOnOneDocument();
                    dbEntityDeclarationExtractor.Visit(root);
                    foreach (var dbEntityDeclaration in dbEntityDeclarationExtractor.DatabaseEntityDeclarations)
                    {
                        DatabaseEntityDeclarations.Add(dbEntityDeclaration);
                    }
                }
            }
        }
    }
}
