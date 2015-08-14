using Detector.Models;
using System;
using System.Collections.Generic;

namespace Detector.Extractors.Base.Observables
{
    //public class ExtractionTracker<T> : IObservable<Extractor<T>> where T : Model
    //{
    //    public ExtractionTracker()
    //    {
    //        observers = new List<IObserver<Extractor<T>>>();
    //    }

    //    private List<IObserver<Extractor<T>>> observers;

    //    public IDisposable Subscribe(IObserver<Extractor<T>> observer)
    //    {
    //        if (!observers.Contains(observer))
    //            observers.Add(observer);
    //        return new Unsubscriber(observers, observer);
    //    }

    //    private class Unsubscriber : IDisposable
    //    {
    //        private List<IObserver<Extractor<T>>> _observers;
    //        private IObserver<Extractor<T>> _observer;

    //        public Unsubscriber(List<IObserver<Extractor<T>>> observers, IObserver<Extractor<T>> observer)
    //        {
    //            this._observers = observers;
    //            this._observer = observer;
    //        }

    //        public void Dispose()
    //        {
    //            if (_observer != null && _observers.Contains(_observer))
    //                _observers.Remove(_observer);
    //        }
    //    }

    //    public void EndTransmission()
    //    {
    //        foreach (var observer in observers.ToArray())
    //            if (observers.Contains(observer))
    //                observer.OnCompleted();

    //        observers.Clear();
    //    }
    //}
}
