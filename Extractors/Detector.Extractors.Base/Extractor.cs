﻿using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors.Base
{
    public abstract class Extractor<T> where T:ORMToolType
    {
        public Context<T> Context { get; }

        public Extractor(Context<T> context)
        {
            Context = context;
        }
    }
}
