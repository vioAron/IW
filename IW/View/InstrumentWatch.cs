using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using IW.Controller;
using IW.Model;

namespace IW.View
{
    public partial class InstrumentWatch : Form, IInstrumentWatch
    {
        private readonly InstrumentWatchController _controller = new InstrumentWatchController();

        private readonly IScheduler _uiScheduler;

        private readonly BindingList<InstrumentMarketData> _dataSource = new BindingList<InstrumentMarketData>();

        public InstrumentWatch()
        {
            InitializeComponent();

            _uiScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
            
            dataGridView1.DataSource = _dataSource;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _controller.InstrumentMarketDataObservable.Buffer(TimeSpan.FromSeconds(2), 10)
                .ObserveOn(_uiScheduler).SubscribeOn(Scheduler.Default)
                .Subscribe(items =>
            {
                foreach (var instrumentMarketData in items)
                {
                    _dataSource.Add(instrumentMarketData);
                }
            });
        }
    }
}