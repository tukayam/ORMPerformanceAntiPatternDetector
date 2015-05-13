using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Models.ORM.Base
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
