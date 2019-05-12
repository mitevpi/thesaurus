#region References

using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Filters;
using Accord.Statistics.Models.Markov.Topology;
using System.Collections.Generic;
using System.IO;
using Accord.IO;
using Accord.MachineLearning.Bayes;
using Accord.Math;
// ReSharper disable InvokeAsExtensionMethod

#endregion

namespace thesaurus
{
    public class TrainModel
    {
        public static string TrainingMode = "bayes"; //switch to "markov" to try the Hidden Markov model

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingData"></param>
        public void TrainHiddenMarkovModel(List<string[]> trainingData)
        {
            Accord.Math.Random.Generator.Seed = 42;

            // Dummy data
            var nodePairs = trainingData.ToArray();

            // Transform data to sequence of integer labels using a codification codebook:
            var codebook = new Codification("Nodes", nodePairs);

            // Create the training data for the models:
            var sequence = codebook.Transform("Nodes", nodePairs);

            // Specify a forward topology
            var topology = new Forward(4);
            var symbols = codebook["Nodes"].NumberOfSymbols;

            // Create the hidden Markov model
            var hmm = new HiddenMarkovModel(topology, symbols);

            // Create the learning algorithm
            var teacher = new BaumWelchLearning(hmm);

            // Teach the model
            teacher.Learn(sequence);

            // Use the Serializer class to save model and codebook
            Serializer.Save(codebook, Path.Combine(ThesaurusViewExtension.ThesaurusDirectory, "thesaurus_codebook.accord"));
            Serializer.Save(hmm, Path.Combine(ThesaurusViewExtension.ThesaurusDirectory, "thesaurus_HMModel.accord"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingData"></param>
        public void TrainNaiveBayesClassifier(List<string[]> trainingData)
        {
            string[] columnNames = { "input", "output" };
            var nodePairs = trainingData.ToArray();
            var codebook = new Codification(columnNames, nodePairs);

            var symbols = codebook.Transform(nodePairs);

            var inputs = symbols.Get(null, 0, -1);
            var outputs = symbols.GetColumn(-1);

            // Create a new Naive Bayes learning
            var learner = new NaiveBayesLearning();

            // Learn a Naive Bayes model from the examples
            var nb = learner.Learn(inputs, outputs);

            // Use the Serializer class to save model and codebook
            Serializer.Save(codebook, Path.Combine(ThesaurusViewExtension.ThesaurusDirectory, "thesaurus_codebook.accord"));
            Serializer.Save(nb, Path.Combine(ThesaurusViewExtension.ThesaurusDirectory, "thesaurus_bayes.accord"));
        }
    }
}