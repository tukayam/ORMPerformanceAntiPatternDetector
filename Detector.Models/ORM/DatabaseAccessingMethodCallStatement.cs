﻿using Detector.Models.Base;
using Detector.Models.Others;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingMethodCallStatement<T> : ModelBase where T : ORMToolType
    {
        /// <summary>
        /// Query that was generated by the provider used. This could be SQL, Oracle or another type of query.
        /// </summary>
        public string ExecutedQuery { get; private set; }

        public ModelCollection<DatabaseEntityDeclaration<T>> EntityDeclarationsUsedInQuery { get; private set; }
        public DatabaseQueryVariable<T> DatabaseQueryVariable { get; private set; }
        public string QueryTextInCSharp { get; private set; }

        public DataContextInitializationStatement<T> DataContext { get; private set; }        

        /// <summary>
        /// Types of entities that will be retrieved with the database accessing call. Eagerly fetching calls will result in more entity declarations to be loaded.
        /// </summary>
        public List<DatabaseEntityDeclaration<T>> LoadedEntityDeclarations { get; private set; }

        /// <summary>
        /// Returns true if the call fetches related entities together with the selected entity.
        /// </summary>
        public bool DoesEagerLoad
        {
            get
            {
                return LoadedEntityDeclarations.Count() > 1;
            }
        }
        
        /// <summary>
        /// Returns variable declaration, if the result of the call is assigned to a variable.
        /// </summary>
        public VariableDeclaration AssignedVariable { get; private set; }

        public CompilationInfo CompilationInfo { get; private set; }

        public DatabaseAccessingMethodCallStatement(string queryTextInCSharp
            , ModelCollection<DatabaseEntityDeclaration<T>> entityDeclarations
            , DatabaseQueryVariable<T> databaseQueryVariable
            , CompilationInfo compilationInfo)
        {
            this.QueryTextInCSharp = queryTextInCSharp;
            this.EntityDeclarationsUsedInQuery = entityDeclarations;
            this.DatabaseQueryVariable = databaseQueryVariable;
            this.CompilationInfo = compilationInfo;

            this.LoadedEntityDeclarations = new List<DatabaseEntityDeclaration<T>>();
        }

        public void SetDataContext(DataContextInitializationStatement<T> dataContext)
        {
            DataContext = dataContext;
        }

        public void SetLoadedEntityDeclarations(List<DatabaseEntityDeclaration<T>> loadedEntityDeclarations)
        {
            LoadedEntityDeclarations = loadedEntityDeclarations;
        }

        public void SetAssignedVariable(VariableDeclaration assignedVariable)
        {
            AssignedVariable = assignedVariable;
        }       
    }
}
