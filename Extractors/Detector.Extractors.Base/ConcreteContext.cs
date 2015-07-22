using Detector.Models.ORM;
using Detector.Models.Others;
using System;

namespace Detector.Extractors.Base
{
    public sealed class ConcreteContext<T> : Context<T> where T : ORMToolType
    {
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        public ModelCollection<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
       
        private static volatile ConcreteContext<T> instance;
        private static object syncRoot = new Object();

        private ConcreteContext()
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

