// <Snippet1>
using Microsoft.ML.Runtime.Api;
// </Snippet1>

namespace TaxiFarePrediction
{
    // <Snippet2>
    public class NodeObject
    {
        [Column("0")]
        public string NodeAName;

        [Column("2")]
        public float CountAllConnections;
        
        [Column("3")]
        public float CountUniqueConnections;
        
        [Column("4")]
        public string NodeType;

        [Column("1")]
        public string NodeBName;

    }

    public class NodePrediction
    {
        [ColumnName("Score")]
        public string NodeBName;
    }
    // </Snippet2>
}
