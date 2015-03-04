using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using IW.Model;

namespace IW.Controller
{
    public class InstrumentWatchController
    {
        public IObservable<InstrumentMarketData> InstrumentMarketDataObservable
        {
            get
            {
                return Observable.Create<InstrumentMarketData>(o =>
                {
                    Observable.Range(0, 100).Subscribe(r => o.OnNext(GetInstrumentMarketData()));
                    
                    return Disposable.Empty;
                });
            }
        }

        private static InstrumentMarketData GetInstrumentMarketData()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(200));

            return InstrumentMarketData.New;
        }
    }
}
