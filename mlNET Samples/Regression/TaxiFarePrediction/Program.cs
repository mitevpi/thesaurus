// <Snippet17>
using System;
using System.IO;
// </Snippet17>
// <Snippet1>
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;
// </Snippet1>
// <Snippet9>
using System.Threading.Tasks;
// </Snippet9>

namespace Regression
{
    class Program
    {
        // <Snippet2>
        static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "graphDataFeaturized.csv");
        static readonly string _testdatapath = Path.Combine(Environment.CurrentDirectory, "Data", "graphDataFeaturized.csv");
        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        // </Snippet2>

        // <Snippet8>
        static async Task Main(string[] args)
        // </Snippet8>
        {
            Console.WriteLine(Environment.CurrentDirectory);
            // <Snippet7>
            PredictionModel<NodeObject, NodePrediction> model = await Train();
            // </Snippet7>

            // <Snippet10>
            //Evaluate(model);
            // </Snippet10>

            // <Snippet16>
            NodePrediction prediction = model.Predict(TestNode.Node1);
            Console.WriteLine("Predicted Node ID: {0}", prediction.NodeIdCounter);
            Console.ReadKey();
            // </Snippet16>
        }

        // <Snippet6>
        public static async Task<PredictionModel<NodeObject, NodePrediction>> Train()
        // </Snippet6>
        {
            // <Snippet3>
            var pipeline = new LearningPipeline
            {
                new TextLoader(_datapath).CreateFrom<NodeObject>(useHeader: true, separator: ','),
                new ColumnCopier(("NodeIdCounter", "Label")),
                new CategoricalOneHotVectorizer(
                    "NodeAName",
                    "NodeBName",
                    "NodeType"),
                new ColumnConcatenator(
                    "Features",
                    "NodeAName",
                    "CountAllConnections",
                    "CountUniqueConnections",
                    "NodeType"),
                new FastTreeRegressor()
                //new NaiveBayesClassifier()
            };
            
            PredictionModel<NodeObject, NodePrediction> model = pipeline.Train<NodeObject, NodePrediction>();
           
            await model.WriteAsync(_modelpath);
            return model;
        }

        private static void Evaluate(PredictionModel<NodeObject, NodePrediction> model)
        {
            // <Snippet12>
            var testData = new TextLoader(_testdatapath).CreateFrom<NodeObject>(useHeader: true, separator: ',');
            // </Snippet12>

            // <Snippet13>
            var evaluator = new RegressionEvaluator();
            RegressionMetrics metrics = evaluator.Evaluate(model, testData);
            // </Snippet13>

            // <Snippet14>
            Console.WriteLine($"Rms = {metrics.Rms}");
            // </Snippet14>
            // <Snippet15>
            Console.WriteLine($"RSquared = {metrics.RSquared}");
            // </Snippet15>
        }
    }
}
