﻿using Detector.Extractors.Base;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Extractors.DatabaseEntities
{
    public abstract class DatabaseEntityDeclarationExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public HashSet<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; }

        public async Task FindDatabaseEntityDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            await ExtractDatabaseEntityDeclarationsAsync(solution, progress);
            Context.DatabaseEntityDeclarations = DatabaseEntityDeclarations;
        }

        protected abstract Task ExtractDatabaseEntityDeclarationsAsync(Solution solution, IProgress<ExtractionProgress> progress);

        public DatabaseEntityDeclarationExtractor(Context<T> context)
            : base(context)
        {
            DatabaseEntityDeclarations = new HashSet<DatabaseEntityDeclaration<T>>();
        }
    }
}
