namespace Detector.Models.AntiPatterns
{
    public interface AntiPatternBase
    {
        ORMModelTree ORMModelTree { get; }
        ModelBase ModelToChange { get; }
    }
}
