using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Base.Helpers;
using Detector.Models.Others;
using System;
using System.Threading.Tasks;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseAccessingMethodCallExtractor : DatabaseAccessingMethodCallExtractor<LINQToSQL>
    {
        public LINQToSQLDatabaseAccessingMethodCallExtractor(Context<LINQToSQL> context)
            : base(context)
        { }
    }
}
