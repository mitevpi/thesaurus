using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiFarePrediction
{
    // <Snippet1>
    static class TestNode
    // </Snippet1>
    {
        // <Snippet2>
        internal static readonly NodeObject Node1 = new NodeObject
        {
            NodeAName = "DSCore.String.Join@string_string[]",
            CountAllConnections = 3,
            CountUniqueConnections = 1,
            NodeType = "DSCore",
            NodeBName = ""// predict it. actual = 29.5
        };
        // </Snippet2>
    }
}
