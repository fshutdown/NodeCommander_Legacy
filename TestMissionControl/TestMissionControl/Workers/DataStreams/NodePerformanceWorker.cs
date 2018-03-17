using System.Threading.Tasks;
using CoinmasterClient;
using CoinMasterAgent;
using TestMissionControl.Agents;

namespace TestMissionControl.Workers.DataStreams
{
    public class NodePerformanceWorker : BaseWorker
    {
        public NodePerformanceWorker(double interval) : base(interval)
        {
        }

        public override void Reset()
        {
        }

        protected override async Task<DataUpdateEventArgs> RunQuery()
        {
            DataUpdateEventArgs args = new DataUpdateEventArgs();
            await Task.Run(() =>
            {
                lock (QueryLock)
                {
                    /*
                    AgentConnection socket = AgentConnection.Create("ws://127.0.0.1:8181");
                    socket.Connect();
                    socket.OnConnect(wrapper =>
                    {
                        args.Data = new AnalysisPackage();


                        MeasureCollection measures = new MeasureCollection();
                        measures.Add(MeasureType.CPU, "100");
                        args.Data.NodeMeasures.Add("RelayNode", measures);
                        socket.OnMessage((s, wrapper1) => measures.Add(MeasureType.Status, s));

                        socket.SendMessage("Running");
                    });
                    */
                }
                    //args.Data = StarrtEbaDiagnostic.GetEbaDataOverlay();
            });
            return args;
        }
       

    }
}
