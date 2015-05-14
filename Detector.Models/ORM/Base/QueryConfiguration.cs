namespace Detector.Models.ORM.Base
{
    public abstract class QueryConfiguration
    {
        public bool LazyLoadingTurnedOff { get; private set; }

        public void TurnLazyLoadingOff()
        {
            this.LazyLoadingTurnedOff = true;
        }
    }
}
