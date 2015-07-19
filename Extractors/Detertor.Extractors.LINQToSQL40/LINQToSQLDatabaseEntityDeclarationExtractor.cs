using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Detector.Models.Others;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<LINQToSQL>
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

                    var dbEntityDeclarationExtractor = new LINQToSQLDatabaseEntityDeclarationExtractorOnOneDocument();
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
