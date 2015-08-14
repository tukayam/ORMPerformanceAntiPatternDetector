using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public abstract class DataContextDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public HashSet<DataContextDeclaration<T>> DataContextDeclarations { get; protected set; }

        public DataContextDeclarationExtractor(Context<T> context)
            : base(context)
        {
            DataContextDeclarations = new HashSet<DataContextDeclaration<T>>();
        }

        public async Task FindDataContextDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            await ExtractDataContextDeclarationsAsync(solution, progress);

            Context.DataContextDeclarations = DataContextDeclarations;
        }

        protected abstract Task ExtractDataContextDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress);
    }
}
