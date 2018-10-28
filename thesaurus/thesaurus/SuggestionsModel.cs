using System;
using Dynamo.ViewModels;
using Dynamo.Graph.Nodes;
using System.Reflection;
using Dynamo.Models;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Filters;
using System.Diagnostics;
using Accord.IO;
using System.IO;

namespace thesaurus
{
    public class SuggestionsModel
    {
        private DynamoViewModel _dynamoViewModel { get; set; }

        public DynamoViewModel DynamoViewModel
        {
            get { return _dynamoViewModel; }
            set { _dynamoViewModel = value; }
        }
        private HiddenMarkovModel loadedHMM;
        private Codification loadedCodebook;

        public SuggestionsModel(DynamoViewModel dvm)
        {
            DynamoViewModel = dvm;
            LoadModel();
        }

        /// <summary>
        /// This handler responds to clicking on the SuggestionNodeButton and create node to the Dynamo
        /// session current workspace
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
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

        private void LoadModel()
        {
            string path = "C:/Users/toulkev/dev/thesaurus/thesaurus/thesaurus/bin/Debug";
            loadedHMM = Serializer.Load<HiddenMarkovModel>(Path.Combine(path, "thesaurus_HMModel.accord"));
            loadedCodebook = Serializer.Load<Codification>(Path.Combine(path, "thesaurus_codebook.accord"));
        }

        public string[] Predict(string nodeName)
        {
            // Test a hard coded example
            int code = loadedCodebook.Transform("Nodes", nodeName);
            int[] predictSample = loadedHMM.Predict(observations: new[] { code }, next: 1);
            string[] predictResult = loadedCodebook.Revert("Nodes", predictSample);
            return predictResult;
        }
    }
}
