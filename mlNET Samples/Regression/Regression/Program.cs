using System;
using System.IO;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;

using System.Threading.Tasks;
namespace Regression
{
    class Program
    {
        // Set Data Paths | TODO: GET TESTING DATA
        static readonly string _datapath = Path.Combine(Environment.CurrentDirectory, "Data", "graphDataFeaturized.csv");
        static readonly string _testdatapath = Path.Combine(Environment.CurrentDirectory, "Data", "graphDataFeaturized.csv");
        static readonly string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        
        static readonly string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        static async Task Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            PredictionModel<NodeObject, NodePrediction> model = await Train();

            //Evaluate(model);

            NodePrediction prediction = model.Predict(TestNode.Node1);
            Console.WriteLine("Predicted Node ID: {0}", prediction.NodeIdCounter);
            Console.ReadKey();
        }


        public static async Task<PredictionModel<NodeObject, NodePrediction>> Train()
        {
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
            TextLoader testData = new TextLoader(_testdatapath).CreateFrom<NodeObject>(useHeader: true, separator: ',');

            RegressionEvaluator evaluator = new RegressionEvaluator();
            RegressionMetrics metrics = evaluator.Evaluate(model, testData);

            Console.WriteLine($"Rms = {metrics.Rms}");
            Console.WriteLine($"RSquared = {metrics.RSquared}");
        }
    }
}
