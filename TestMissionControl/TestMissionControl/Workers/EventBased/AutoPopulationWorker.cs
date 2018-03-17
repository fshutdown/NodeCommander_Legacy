using System.Threading.Tasks;

namespace TestMissionControl.Workers.EventBased
{
    public class AutoPopulationWorker : BaseWorker
    {
        public AutoPopulationWorker(double interval) : base(interval)
        {
            DisabledByDefault = true;
        }

        public override void Reset()
        {
        }

        protected override async Task<DataUpdateEventArgs> RunQuery()
        {
            DataUpdateEventArgs args = new DataUpdateEventArgs();
            await Task.Run(() =>
            {
                //lock (QueryLock)
                //    args.Data = StarrtOscar.GetOrphanSourceKeys();
            });
            return args;
        }


        public void Refresh()
        {
            Reset();
            Tick();
        }
    }
}
