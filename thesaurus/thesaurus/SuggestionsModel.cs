using System;
using Dynamo.ViewModels;
using Dynamo.Graph.Nodes;
using System.Reflection;
using Dynamo.Models;

namespace thesaurus
{
    public class SuggestionsModel
    {
        private DynamoViewModel DynamoViewModel { get; set; }

        public SuggestionsModel(DynamoViewModel dvm)
        {
            DynamoViewModel = dvm;
        }

        public bool PlaceNode(string nodeName)
        {
            // Get Reference of DynamoModel
            var dm = DynamoViewModel.Model;
            var nsm = dm.SearchModel;

            foreach (var se in nsm.SearchEntries)
            {
                

                if (se.FullName.EndsWith(nodeName, StringComparison.OrdinalIgnoreCase))
                {
                    var dynMethod = se.GetType().GetMethod("ConstructNewNodeModel",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    var obj = dynMethod.Invoke(se, new object[] { });
                    var nM = obj as NodeModel;

                    try
                    {
                        dm.ExecuteCommand(new DynamoModel.CreateNodeCommand(nM, 0, 0, true, false));
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
