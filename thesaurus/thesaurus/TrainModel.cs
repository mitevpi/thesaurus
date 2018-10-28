﻿using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;
using Accord.Statistics.Filters;
using Accord.Statistics.Models.Markov.Topology;
using System.Collections.Generic;
using Accord.IO;

namespace thesaurus
{
    public class TrainModel
    {
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
            Serializer.Save(codebook, "thesaurus_codebook.accord");
            Serializer.Save(hmm, "thesaurus_HMModel.accord");
        }
    }
}