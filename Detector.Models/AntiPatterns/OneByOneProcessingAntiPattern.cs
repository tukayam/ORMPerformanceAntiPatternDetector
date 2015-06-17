namespace Detector.Models.AntiPatterns
{
    public class OneByOneProcessingAntiPattern : AntiPatternBase
    {
        public ModelBase ModelToChange { get; private set; }
        public ORMModelTree ORMModelTree { get; private set; }

        public OneByOneProcessingAntiPattern(ModelBase modelToChange, ORMModelTree ormModelTree)
        {
            ModelToChange = modelToChange;
            ORMModelTree = ormModelTree;
        }
    }
}
