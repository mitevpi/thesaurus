using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Filters;
using Accord.Statistics.Models.Markov.Topology;
using System.Collections.Generic;
using Accord.IO;
using Accord.MachineLearning.Bayes;

using Accord.Math;
using System.Data;


namespace thesaurus
{
    public class TrainModel
    {
        public static string trainingMode = "bayes"; //switch to "markov" to try the Hidden Markov model
        public TrainModel()
        {

        }

        public void TrainHiddenMarkovModel(List<string[]> trainingData)
        {

            Accord.Math.Random.Generator.Seed = 42;

            string[][] nodePairs = trainingData.ToArray();

            // Transform data to sequence of integer labels using a codification codebook:
            var codebook = new Codification("Nodes", nodePairs);

            // Create the training data for the models:
            int[][] sequence = codebook.Transform("Nodes", nodePairs);

            // Specify a forward topology
            var topology = new Forward(states: 4);
            int symbols = codebook["Nodes"].NumberOfSymbols;

            // Create the hidden Markov model
            var hmm = new HiddenMarkovModel(topology, symbols);

            // Create the learning algorithm
            var teacher = new BaumWelchLearning(hmm);

            // Teach the model
            teacher.Learn(sequence);

            // Use the Serializer class to save model and codebook
            Serializer.Save(obj: codebook, path: "thesaurus_codebook.accord");
            Serializer.Save(obj: hmm, path: "thesaurus_HMModel.accord");
        }

        public void TrainNaiveBayesClassifier(List<string[]> trainingData)
        {
            string[] columnNames = { "input", "output" };
            string[][] nodePairs = trainingData.ToArray();
            Codification codebook = new Codification(columnNames, nodePairs);
            
            int[][] symbols = codebook.Transform(nodePairs);

            int[][] inputs = symbols.Get(null, 0, -1);
            int[] outputs = symbols.GetColumn(-1);

            // Create a new Naive Bayes learning
            var learner = new NaiveBayesLearning();

            // Learn a Naive Bayes model from the examples
            NaiveBayes nb = learner.Learn(inputs, outputs);

            // Use the Serializer class to save model and codebook
            Serializer.Save(obj: codebook, path: "thesaurus_codebook.accord");
            Serializer.Save(obj: nb, path: "thesaurus_bayes.accord");
        }
    }
}
