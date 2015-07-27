using Detector.Models.ORM;
using Detector.Models.Others;
using System;

namespace Detector.Extractors.Base
{
    public sealed class ConcreteContext<T> : Context<T> where T : ORMToolType
    {
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        public ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        public ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
        public ModelCollection<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; set; }

        private static volatile ConcreteContext<T> instance;
        private static object syncRoot = new Object();

        public ConcreteContext()
        {
            DataContextDeclarations = new ModelCollection<DataContextDeclaration<T>>();
            DatabaseEntityDeclarations = new ModelCollection<DatabaseEntityDeclaration<T>>();
        }

        public static ConcreteContext<T> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConcreteContext<T>();
                    }
                }

                return instance;
            }
        }
    }
}

