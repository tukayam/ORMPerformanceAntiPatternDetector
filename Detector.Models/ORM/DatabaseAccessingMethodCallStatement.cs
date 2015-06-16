using Detector.Models.Base;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Detector.Models.ORM
{
    public class DatabaseAccessingMethodCallStatement<T> : ModelBase where T : ORMToolType
    {
        public DatabaseQuery<T> DatabaseQuery { get; private set; }
        
        public DataContextInitializationStatement<T> DataContext { get; private set; }        

        /// <summary>
        /// Types of entities that will be retrieved with the database accessing call. Eagerly fetching calls will result in more entity declarations to be loaded.
        /// </summary>
        public IEnumerable<DatabaseEntityDeclaration<T>> LoadedEntityDeclarations { get; private set; }

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

        public DatabaseAccessingMethodCallStatement(DatabaseQuery<T> databaseQuery, CompilationInfo compilationInfo )
        {
            this.CompilationInfo = compilationInfo;
            DatabaseQuery = databaseQuery;
        }

        public void SetDataContext(DataContextInitializationStatement<T> dataContext)
        {
            DataContext = dataContext;
        }

        public void SetLoadedEntityDeclarations(IEnumerable<DatabaseEntityDeclaration<T>> loadedEntityDeclarations)
        {
            LoadedEntityDeclarations = loadedEntityDeclarations;
        }

        public void SetAssignedVariable(VariableDeclaration assignedVariable)
        {
            AssignedVariable = assignedVariable;
        }
    }
}
