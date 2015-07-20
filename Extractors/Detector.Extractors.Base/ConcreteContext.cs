using Detector.Models.ORM;
using Detector.Models.Others;
using System;

namespace Detector.Extractors.Base
{
    public sealed class ConcreteContext<T> : Context<T> where T:ORMToolType
    {
        private ModelCollection<DataContextDeclaration<T>> _dataContextDeclarations;
        public ModelCollection<DataContextDeclaration<T>> DataContextDeclarations
        {
            get
            {
                return _dataContextDeclarations;
            }

            set
            {
                _dataContextDeclarations = value;
            }
        }

        private static volatile ConcreteContext<T> instance;
        private static object syncRoot = new Object();

        private ConcreteContext() { }

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

