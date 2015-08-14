using Detector.Models;
using System;

namespace Detector.Extractors.Base.Observers
{
    //public class ExtractorReporter<T> : IObserver<Extractor<T>> where T:Model
    //{
    //    private IDisposable unsubscriber;
    //    private string instName;

    //    public ExtractorReporter(string name)
    //    {
    //        this.instName = name;
    //    }

    //    public string Name
    //    { get { return this.instName; } }

    //    public virtual void Subscribe(IObservable<Extractor<T>> provider)
    //    {
    //        if (provider != null)
    //            unsubscriber = provider.Subscribe(this);
    //    }

    //    public virtual void OnCompleted()
    //    {
    //        Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", this.Name);
    //        this.Unsubscribe();
    //    }

    //    public virtual void OnError(Exception e)
    //    {
    //        Console.WriteLine("{0}: The location cannot be determined.", this.Name);
    //    }

    //    public virtual void OnNext(Extractor<T> value)
    //    {
    //        //    Console.WriteLine("{2}: The current location is {0}, {1}", value., value.Longitude, this.Name);
    //    }

    //    public virtual void Unsubscribe()
    //    {
    //        unsubscriber.Dispose();
    //    }
    //}
}
