using System;
using System.Collections.Generic;
using System.Text;

namespace Regression
{
    // <Snippet1>
    static class TestNode
    // </Snippet1>
    {
        // <Snippet2>
        internal static readonly NodeObject Node1 = new NodeObject
        {
            NodeAName = "DSCore.List.AddItemToFront@var[]..[]_var[]..[]\r\n",
            CountAllConnections = 1,
            CountUniqueConnections = 0,
            NodeType = "DSCore",
            NodeBName = ""// predict it. actual = 2who the hell knows man
        };
        // </Snippet2>
    }
}
