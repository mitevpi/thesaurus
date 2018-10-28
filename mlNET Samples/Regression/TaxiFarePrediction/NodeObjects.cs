// <Snippet1>
using Microsoft.ML.Runtime.Api;
// </Snippet1>

namespace Regression
{
    // <Snippet2>
    public class NodeObject
    {
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
    // </Snippet2>
}
