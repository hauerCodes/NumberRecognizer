﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberRecognizer.Lib.Network
{
    [Serializable]
    public class HiddenNeuron : INeuron
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenNeuron"/> class.
        /// </summary>
        public HiddenNeuron()
        {
            InputLayer = new List<WeightedLink>();
        }

        /// <summary>
        /// Gets the activation value.
        /// </summary>
        /// <value>
        /// The activation value.
        /// </value>
        public double ActivationValue
        {
            get
            {
                double sum = InputLayer.Sum(x => x.Neuron.ActivationValue * x.Weight);

                //Sigmoid
                return ((1 / (1 + Math.Pow(Math.E, sum * -1))) * 2) - 1;
            }
        }

        /// <summary>
        /// Gets or sets the input layer.
        /// </summary>
        /// <value>
        /// The input layer.
        /// </value>
        public List<WeightedLink> InputLayer { get; private set; }
    }
}
