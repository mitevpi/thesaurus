using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Filters;
using Accord.Statistics.Models.Markov.Topology;
using System.Diagnostics;
using Accord.IO;

namespace thesaurus
{
    public class TrainModel
    {
        public TrainModel()
        {

        }

        static void TrainHiddenMarkovModel()
        {

            Accord.Math.Random.Generator.Seed = 42;

            // Dummy data
            string[][] nodePairs =
            {
                new[] { "int", "point"},
                new[] { "int", "point"},
                new[] { "point", "line"},
                new[] { "point", "radius"},
                new[] { "int", "list"},
            };

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

     
            // TEST
            int code = codebook.Transform("Nodes", "int");
            int[] predictSample = hmm.Predict(observations: new[] { code }, next: 1);
            // And the result will be: "those", "are", "words".
            string[] predictResult = codebook.Revert("Nodes", predictSample);

            string seed = "int -> ";
            foreach (string node in predictResult)
            {
                seed += node;
            }
            Debug.WriteLine(seed);

            // Use the Serializer class to save model and codebook
            Serializer.Save(obj: codebook, path: "thesaurus_codebook.accord");
            Serializer.Save(obj: hmm, path: "thesaurus_HMModel.accord");
        }

    }
}
