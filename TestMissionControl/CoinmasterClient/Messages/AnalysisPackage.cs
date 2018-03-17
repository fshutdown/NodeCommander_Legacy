using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinmasterClient
{
    public class AnalysisPackage
    {
        public Dictionary<string, MeasureCollection> NodeMeasures { get; set; }

        public AnalysisPackage()
        {
            NodeMeasures = new Dictionary<string, MeasureCollection>();
        }
    }
}
