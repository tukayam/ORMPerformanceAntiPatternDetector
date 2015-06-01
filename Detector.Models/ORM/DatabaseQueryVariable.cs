namespace Detector.Models.ORM
{
    public class DatabaseQueryVariable
    {
        public string VariableName { get; private set; }

        public DatabaseQueryVariable(string variableName)
        {
            this.VariableName = variableName;
        }
    }
}
