namespace Detector.Models.ORM
{
    public class VariableDeclarationInsideDatabaseAccessingLoop<T> where T : ORMToolType
    {
        public string VariableName { get; private set; }
        public DatabaseEntityDeclaration<T> DatabaseEntityDeclaration { get; private set; }

        public VariableDeclarationInsideDatabaseAccessingLoop(string variableName, DatabaseEntityDeclaration<T> databaseEntityDeclaration)
        {
            this.VariableName = variableName;
            this.DatabaseEntityDeclaration = databaseEntityDeclaration;
        }
    }
}
