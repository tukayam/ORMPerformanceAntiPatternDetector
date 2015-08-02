namespace Detector.Models.Base
{
    public abstract class ModelBase : Model
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public ModelBase(CompilationInfo compilationInfo)
        {
            CompilationInfo = compilationInfo;
        }
    }
}
