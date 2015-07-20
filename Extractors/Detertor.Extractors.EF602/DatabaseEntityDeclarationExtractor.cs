using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Extractors.Base;

namespace Detector.Extractors.EF602
{
    public class DatabaseEntityDeclarationExtractor : DatabaseEntityDeclarationExtractor<EntityFramework>
    {
        public DatabaseEntityDeclarationExtractor(Context<EntityFramework> context)
            : base(context)
        { }

        public override ModelCollection<DatabaseEntityDeclaration<EntityFramework>> DatabaseEntityDeclarations { get; }

        public override async Task FindDatabaseEntityDeclarationsAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await document.GetSyntaxRootAsync();

                    //Go through the datacontext declarations and get all DbSet typed properties and then the type T used in DbSet<T>

                    //foreach (var dbEntityDeclaration in dbEntityDeclarationExtractor.DatabaseEntityDeclarations)
                    //{
                    //    DatabaseEntityDeclarations.Add(dbEntityDeclaration);
                    //}
                }
            }
        }

        //private ModelCollection<DatabaseEntityDeclaration<EntityFramework>> FindDatabaseEntityDeclarationsInDocument(SyntaxNode node)
        //{
        //    var dataContextDeclarations = this.Context.DataContextDeclarations;

        //    if (dataContextDeclarations.Count < 1)
        //    {
        //        throw new ArgumentException("DataContextDeclarations are required before DatabaseEntityDeclarations can be detected for a solution using Entity Framework.");
        //    }

        //    if (node.AttributeLists.ToString().Contains("TableAttribute"))
        //    {
        //        _entities.Add(new DatabaseEntityDeclaration<EntityFramework>(node.Identifier.ToString()) { });
        //    }
        //}
    }
}
