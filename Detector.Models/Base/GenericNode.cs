namespace Detector.Models.Base
{
    public interface Node<T> where T: ModelBase
    {
        T Model { get; }        
    }
}
