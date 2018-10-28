using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParseJSON
{
    public class NodeDataModel
    {
        /// <summary>
        /// A JSON serialized string representing this class object.
        /// </summary>
        [NonSerialized()] public readonly string Serialized;

        /// <summary>
        /// The JSON object representation of this class.
        /// </summary>
        [NonSerialized()] public JObject JsonObject;

        public string NodeAName { get; set; }
        public string NodeBName { get; set; }
        public string NodeAId { get; set; }
        public string NodeBId { get; set; }

        public int TotalConnectionsCount { get; set; }
        public int UniqueConnectionsCount { get; set; }

        public bool CoreNode { get; set; }
        public bool CustomNode { get; set; }
        public bool ZeroTouchNode { get; set; }

        public NodeDataModel()
        {
            Serialized = JsonConvert.SerializeObject(this);
            JsonObject = JObject.Parse(Serialized);
        }


        public static NodeDataModel CreateNewDataModel(string NodeASig, string NodeBSig, string NodeBID, string NodeAID)
        {
            NodeDataModel newRecord = new NodeDataModel();
            newRecord.NodeAName = NodeASig.ToString(); newRecord.NodeBName = NodeBSig.ToString();
            newRecord.NodeAId = NodeAID; newRecord.NodeBId = NodeBID;
            newRecord.TotalConnectionsCount = 0;
            newRecord.UniqueConnectionsCount = 0;

            return newRecord;
        }

        public static void ParseNodeDataModels(List<NodeDataModel> modelsList)
        {
            JObject parentJson = new JObject();
            int counter = 0;
            foreach (NodeDataModel dataModel in modelsList)
            {

                string uniqueId = dataModel.NodeAName + counter.ToString();
                parentJson.Add(uniqueId, dataModel.JsonObject);
                counter = counter + 1;
            }

            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Set the output directory to the desktop
            File.WriteAllText(dirPath + "\\graphDataFeaturized.json", parentJson.ToString()); // Export the parent JSON as a static file
        }
    }

}