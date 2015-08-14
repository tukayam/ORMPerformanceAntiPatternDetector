namespace Detector.Extractors.Base.ExtractionStrategies
{
    public interface Parameter
    {
        string Name { get; }
    }

    public interface Parameter<T> : Parameter
    {
        T Value { get; }
    }

    public abstract class ParameterBase<T> : Parameter<T>
    {
        public string Name { get; protected set; }
        public T Value { get; protected set; }

        public ParameterBase(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
