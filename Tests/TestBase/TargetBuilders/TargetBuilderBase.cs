using Detector.Extractors.Base;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using System;
using System.Collections.Generic;
using TestBase.Stubs;

namespace TestBase.TargetBuilders
{
    public abstract class TargetBuilderBase<T, Y> where T : ORMToolType where Y : Extractor<T>, new()
    {
        private Context<T> _context;
        private Y _target;

        /// <summary>
        /// Should be called providing the delegate to create the Extractor
        /// For example: new ConcreteTargetBuilder<LINQToSQL,DatabaseContextDeclarationExtractor<LINQToSQL>>(e=> new DatabaseContextDeclarationExtractor(e));
        /// </summary>
        /// <param name="contextCreationDelegate"></param>
        public TargetBuilderBase(Func<Context<T>, Y> contextCreationDelegate)
        {
            _context = new ContextStub<T>();
            _target = contextCreationDelegate(_context);
        }

        public void SetDataContextDeclarationsInContext(HashSet<DataContextDeclaration<T>> dataContextDeclarations)
        {
            _context.DataContextDeclarations = dataContextDeclarations;
        }
    }
}
