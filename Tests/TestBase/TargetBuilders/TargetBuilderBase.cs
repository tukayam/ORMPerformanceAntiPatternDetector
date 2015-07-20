using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using System;
using TestBase.Stubs;

namespace TestBase.TargetBuilders
{
    public abstract class TargetBuilderBase<T,Y> where T:ORMToolType where Y:Extractor<T>,new()
    {
        private Context<T> _context;
        private Y _target;

        /// <summary>
        /// Should be called providing the delegate to create the Extractor
        /// For example: new ConcreteTargetBuilder<LINQToSQL,DatabaseContextDeclarationExtractor<LINQToSQL>>(e=> new DatabaseContextDeclarationExtractor(e));
        /// </summary>
        /// <param name="contextCreationDelegate"></param>
        public TargetBuilderBase(Func<Context<T>,Y> contextCreationDelegate)
        {
            _context = new ContextStub<T>();
            _target = contextCreationDelegate(_context);
        }

        public void SetDataContextDeclarationsInContext(ModelCollection<DataContextDeclaration<T>> dataContextDeclarations)
        {
            _context.DataContextDeclarations = dataContextDeclarations;
        }
    }
}
