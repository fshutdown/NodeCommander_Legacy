namespace CoinmasterClient.Network
{
    public class SingleNode
    {
        #region Configuration Options
        public string NodeName { get; private set; }
        public string DisplayName { get; set; }
        public string DataDir { get; set; }
        public string Agent { get; set; }
        #endregion

        public MeasureCollection NodeMeasures;


        public SingleNode(string nodeName)
        {
            NodeName = nodeName;
            DisplayName = nodeName;
            NodeMeasures = new MeasureCollection();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
