using System;

namespace Detector.Models.AntiPatterns
{
    public class ExcessiveDataAntiPattern : AntiPatternBase
    {
        public ModelBase ModelToChange { get; private set; }
        public ORMModelTree ORMModelTree { get; private set; }

        public ExcessiveDataAntiPattern(ModelBase modelToChange, ORMModelTree ormModelTree)
        {
            ModelToChange = modelToChange;
            ORMModelTree = ormModelTree;
        }
    }
}
