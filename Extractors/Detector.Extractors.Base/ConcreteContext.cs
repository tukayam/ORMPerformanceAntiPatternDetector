using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.DataContexts;
using Detector.Models.ORM.ORMTools;
using System;
using System.Collections.Generic;
using Detector.Models.Base;

namespace Detector.Extractors.Base
{
    public sealed class ConcreteContext<T> : Context<T> where T : ORMToolType
    {
        public HashSet<DataContextDeclaration<T>> DataContextDeclarations { get; set; }
        public HashSet<DatabaseEntityDeclaration<T>> DatabaseEntityDeclarations { get; set; }
        public HashSet<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCallStatements { get; set; }
        public HashSet<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; set; }
        public HashSet<CodeExecutionPath> CodeExecutionPaths { get; set; }

        private static volatile ConcreteContext<T> instance;
        private static object syncRoot = new Object();

        public ConcreteContext()
        {
            DataContextDeclarations = new HashSet<DataContextDeclaration<T>>();
            DatabaseEntityDeclarations = new HashSet<DatabaseEntityDeclaration<T>>();
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

