using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Extractors.DatabaseEntities
{
    public interface DatabaseEntityDeclarationExtractor<T> : Extractor where T : ORMToolType
    {
        ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; }
        Task<ModelCollection<DatabaseEntityDeclaration<T>>> ExtractAsync(Solution solution);
    }
}
