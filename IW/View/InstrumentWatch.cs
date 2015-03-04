using System;
using System.ComponentModel;
using System.Globalization;
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
        private IDisposable _loadInstrumentsSubscription;

        public InstrumentWatch()
        {
            InitializeComponent();

            _uiScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);

            dataGridView1.DataSource = _dataSource;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _loadInstrumentsSubscription.Dispose();

                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnLoad.Enabled = false;
            progressBar1.Value = 0;

            _loadInstrumentsSubscription = _controller.GetInstrumentMarketDataObservable(Enumerable.Range(0, 100).Select(n => n.ToString(CultureInfo.InvariantCulture)))
                .Buffer(TimeSpan.FromSeconds(2), 10)
                .ObserveOn(_uiScheduler).SubscribeOn(Scheduler.Default)
                .Finally(() => btnLoad.Enabled = true)
                .Subscribe(items =>
                {
                    foreach (var instrumentMarketData in items)
                    {
                        _dataSource.Add(instrumentMarketData);
                    }

                    progressBar1.Value = _dataSource.Count % 101;
                }, ex => MessageBox.Show(ex.Message));
        }
    }
}