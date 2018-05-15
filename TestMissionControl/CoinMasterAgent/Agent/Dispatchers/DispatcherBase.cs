using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Stratis.CoinMasterAgent.Agent.Dispatchers.EventArgs;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers
{
    public abstract class DispatcherBase
    {
        public delegate void UpdateHandler(DispatcherBase sender, UpdateEventArgs args);
        public event UpdateHandler Updated;
        public AgentSession Session { get; set; }

        protected Timer _jobScheduler;
        protected double _interval;
        public bool Enabled { get; protected set; }

        public Double Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                _jobScheduler.Interval = value;
            }
        }

        public DispatcherBase(AgentSession session, double interval)
        {
            Session = session;

            _jobScheduler = new Timer();
            _jobScheduler.Elapsed += PerformWork;
            _jobScheduler.AutoReset = true;
            Interval = interval;
        }

        public abstract void Reset();
        public abstract void SendData();
        public abstract void Close();

        public void Start()
        {
            Reset();
            Enabled = true;
            _jobScheduler.Interval = 100;
            _jobScheduler.Start();
        }
        
        public void Stop()
        {
            Enabled = false;
            _jobScheduler.Enabled = false;
            _jobScheduler.Stop();
            Close();
        }


        public void Tick(bool suspendTimer = false)
        {
            _jobScheduler.Stop();
            _jobScheduler.Interval = 100;

            if (!suspendTimer) _jobScheduler.Start();
            else PerformWork(this, null);
        }


        private void PerformWork(object sender, ElapsedEventArgs e)
        {
            _jobScheduler.Interval = _interval;
            _jobScheduler.Stop();

            if (!Enabled) return;
            SendData();

            _jobScheduler.Start();
        }
        
        protected void OnUpdate(DispatcherBase sender, UpdateEventArgs args)
        {
            Updated?.Invoke(sender, args);
        }
    }
}
