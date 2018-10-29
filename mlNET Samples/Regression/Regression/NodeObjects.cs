using Microsoft.ML.Runtime.Api;

namespace Regression
{
    public class NodeObject
    {
        // [Column("X")] syntax denotes index of the column from the CSV loaded in.
        [Column("0")]
        public string NodeAName;

        [Column("1")]
        public string NodeBName;

        [Column("2")]
        public float CountAllConnections;
        
        [Column("3")]
        public float CountUniqueConnections;
        
        [Column("4")]
        public string NodeType;

        [Column("7")]
        public float NodeIdCounter;

    }

    public class NodePrediction
    {
        [ColumnName("Score")]
        public float NodeIdCounter;
    }
}
