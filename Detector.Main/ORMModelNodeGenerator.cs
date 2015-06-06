using Detector.Models.ORM;
using System;
using Detector.Models;

namespace Detector.Main
{
    public class ORMModelNodeGenerator<T> : ORMModelNodeGeneratorBase<T> where T : ORMToolType
    {
        public ORMModelNode Visit(ModelBase model)
        {
            string type = model.GetType().ToString();
            switch (type)
            {
                case "DatabaseAccessingForeachLoopDeclaration":
                    return this.Visit(model as DatabaseAccessingForeachLoopDeclaration<T>);
                default:
                    throw new ArgumentException(string.Format("No Visit method implemented for type {0}", type));
            }
        }

        public ORMModelNode Visit(DatabaseAccessingForeachLoopDeclaration<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseAccessingDoWhileLoopDeclaration<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseAccessingMethodCallStatementOnQueryDeclaration<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseEntityObjectInstantiationStatement<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseEntityObjectRelatedEntitySelectCallStatement<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseQueryVariable model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(VariableDeclarationInsideDatabaseAccessingLoop<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DataContextInitializationStatement<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseEntityObjectUpdateStatement<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseEntityObjectCallStatement<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseAccessingMethodCallStatementOnQueryVariable<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseAccessingWhileLoopDeclaration<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseAccessingForLoopDeclaration<T> model)
        {
            throw new NotImplementedException();
        }

        public ORMModelNode Visit(DatabaseQuery<T> model)
        {
            throw new NotImplementedException();
        }
    }
}
