namespace ParseJSON
{
    public class NodeDataModel
    {
        public string NodeAName { get; set; }
        public string NodeBName { get; set; }
        public string NodeAId { get; set; }
        public string NodeBId { get; set; }

        public int TotalConnectionsCount { get; set; }
        public int UniqueConnectionsCount { get; set; }

        public bool CoreNode { get; set; }
        public bool CustomNode { get; set; }
        public bool ZeroTouchNode { get; set; }
    }
}