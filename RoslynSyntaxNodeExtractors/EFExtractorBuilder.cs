using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public class EFExtractorBuilder
    {
        private ExtractorBuilder<EntityFramework> _builder;

        public EFExtractorBuilder(Solution solution)
        {
            _builder = new ExtractorBuilder<EntityFramework>(solution)
                .WithDataContextDeclarationsDerivedFromBaseClass("DbContext");
        }

        public EFExtractorBuilder WithDbEntitiesDerivedFromBaseClass(string baseClassName)
        {
            _builder.WithDbEntitiesDerivedFromBaseClass(baseClassName);
            return this;
        }

        public EFExtractorBuilder WithDbEntitiesAsGenericTypeOnIQueryablesInDataContextClasses()
        {
            _builder.WithDbEntitiesAsGenericTypeOnIQueryablesInDataContextClasses();
            return this;
        }

        public async Task Extract()
        {
            await _builder.Extract();
        }
    }
}
