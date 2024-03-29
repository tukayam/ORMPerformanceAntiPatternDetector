﻿using Detector.Extractors.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors.EF602
{
    public class DatabaseAccessingMethodCallExtractor : DatabaseAccessingMethodCallExtractor<EntityFramework>
    {
        public DatabaseAccessingMethodCallExtractor(Context<EntityFramework> context)
            : base(context)
        { }
    }
}
