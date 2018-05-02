using System;
using System.Collections.Generic;
using System.Text;

namespace GridWorld
{
    public class NeuralNetwork
    {
        private static Random rnd;

        private int numInput;
        private int numHidden1;
        private int numHidden2;
        private int numOutput;

        private double[] inputs;

        private double[][] ihWeights; // input-hidden1
        private double[] h1Biases;
        private double[] h1Outputs;

        private double[][] hhWeights; // hidden1-hidden2
        private double[] h2Biases;
        private double[] h2Outputs;

        private double[][] hoWeights; // hidden2-output
        private double[] oBiases;

        private double[] outputs;

    

        public NeuralNetwork(int numInput, int numHidden1, int numHidden2, int numOutput)
        {
            rnd = new Random(0); // for InitializeWeights() and Shuffle()

            this.numInput = numInput;
            this.numHidden1 = numHidden1;
            this.numHidden2 = numHidden2;
            this.numOutput = numOutput;

            this.inputs = new double[numInput];

            this.ihWeights = MakeMatrix(numInput, numHidden1);
            this.h1Biases = new double[numHidden1];
            this.h1Outputs = new double[numHidden1];

            this.hhWeights = MakeMatrix(numHidden1, numHidden2);
            this.h2Biases = new double[numHidden2];
            this.h2Outputs = new double[numHidden2];

            this.hoWeights = MakeMatrix(numHidden2, numOutput);
            this.oBiases = new double[numOutput];

            this.outputs = new double[numOutput];

        } // constructor

        private static double[][] MakeMatrix(int rows, int cols) // helper for constructor
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
                result[r] = new double[cols];
            return result;
        }

        public override string ToString() // yikes
        {
            string s = "";
            s += "===============================\\n";
            s += "numInput = " + numInput + " numHidden1 = " + numHidden1 + " numHidden2 = " + numHidden2 + " numOutput = " + numOutput + "\\n\\n";

            s += "inputs: \\n";
            for (int i = 0; i < inputs.Length; ++i)
                s += inputs[i].ToString("F2") + " ";
            s += "\\n\\n";

            s += "ihWeights: \\n";
            for (int i = 0; i < ihWeights.Length; ++i)
            {
                for (int j = 0; j < ihWeights[i].Length; ++j)
                {
                    s += ihWeights[i][j].ToString("F4") + " ";
                }
                s += "\\n";
            }
            s += "\\n";

            s += "h1Biases: \\n";
            for (int i = 0; i < h1Biases.Length; ++i)
                s += h1Biases[i].ToString("F4") + " ";
            s += "\\n\\n";

            s += "h1Outputs: \\n";
            for (int i = 0; i < h1Outputs.Length; ++i)
                s += h1Outputs[i].ToString("F4") + " ";
            s += "\\n\\n";

            s += "hhWeights: \\n";
            for (int i = 0; i < hhWeights.Length; ++i)
            {
                for (int j = 0; j < hhWeights[i].Length; ++j)
                {
                    s += hhWeights[i][j].ToString("F4") + " ";
                }
                s += "\\n";
            }
            s += "\\n";

            s += "h2Biases: \\n";
            for (int i = 0; i < h2Biases.Length; ++i)
                s += h2Biases[i].ToString("F4") + " ";
            s += "\\n\\n";

            s += "h2Outputs: \\n";
            for (int i = 0; i < h2Outputs.Length; ++i)
                s += h2Outputs[i].ToString("F4") + " ";
            s += "\\n\\n";

            s += "hoWeights: \\n";
            for (int i = 0; i < hoWeights.Length; ++i)
            {
                for (int j = 0; j < hoWeights[i].Length; ++j)
                {
                    s += hoWeights[i][j].ToString("F4") + " ";
                }
                s += "\\n";
            }
            s += "\\n";

            s += "oBiases: \\n";
            for (int i = 0; i < oBiases.Length; ++i)
                s += oBiases[i].ToString("F4") + " ";
            s += "\\n\\n";


            s += "outputs: \\n";
            for (int i = 0; i < outputs.Length; ++i)
                s += outputs[i].ToString("F2") + " ";
            s += "\\n\\n";

            s += "===============================\\n";
            return s;
        }

        // ----------------------------------------------------------------------------------------

        public void SetWeights(double[] weights)
        {
            // Copy weights and biases in weights[] array to i-h1 weights, i-h1 biases, h1-h2 weights, h2-o weights, h1-h2 biases, h2-o biases
            int numWeights = (numInput * numHidden1) + (numHidden1 * numHidden2) + (numHidden2 * numOutput) + numHidden1 + numHidden2 + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("Bad weights array length: ");

            int k = 0; // points into weights param

            for (int i = 0; i < numInput; ++i)
                for (int j = 0; j < numHidden1; ++j)
                    ihWeights[i][j] = weights[k++];
            for (int i = 0; i < numHidden1; ++i)
                h1Biases[i] = weights[k++];  //Bias
            for (int i = 0; i < numHidden1; ++i)
                for (int j = 0; j < numHidden2; ++j)
                    hhWeights[i][j] = weights[k++];
            for (int i = 0; i < numHidden2; ++i)
                h2Biases[i] = weights[k++];  //Bias
            for (int i = 0; i < numHidden2; ++i)
                for (int j = 0; j < numOutput; ++j)
                    hoWeights[i][j] = weights[k++];
            for (int i = 0; i < numOutput; ++i)
                oBiases[i] = weights[k++];  //Bias
        }

        public void InitializeWeights()
        {
            // Initialize weights and biases to small random values
            int numWeights = (numInput * numHidden1) + (numHidden1 * numHidden2) + (numHidden2 * numOutput) + numHidden1 + numHidden2 + numOutput;
            double[] initialWeights = new double[numWeights];
            double lo = -1;
            double hi = 1;
            for (int i = 0; i < initialWeights.Length; ++i)
                initialWeights[i] = (hi - lo) * rnd.NextDouble() + lo;
            this.SetWeights(initialWeights);
        }

        public double[] GetWeights()
        {
            // Returns the current set of wweights, presumably after training
            int numWeights = (numInput * numHidden1) + (numHidden1 * numHidden2) + (numHidden2 * numOutput) + numHidden1 + numHidden2 + numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
                for (int j = 0; j < ihWeights[0].Length; ++j)
                    result[k++] = ihWeights[i][j];
            for (int i = 0; i < h1Biases.Length; ++i)
                result[k++] = h1Biases[i];
            for (int i = 0; i < hhWeights.Length; ++i)
                for (int j = 0; j < hhWeights[0].Length; ++j)
                    result[k++] = hhWeights[i][j];
            for (int i = 0; i < h2Biases.Length; ++i)
                result[k++] = h2Biases[i];
            for (int i = 0; i < hoWeights.Length; ++i)
                for (int j = 0; j < hoWeights[0].Length; ++j)
                    result[k++] = hoWeights[i][j];
            for (int i = 0; i < oBiases.Length; ++i)
                result[k++] = oBiases[i];
            return result;
        }

        // ----------------------------------------------------------------------------------------
        //The function for computing the output layer values
        public double[] ComputeOutputs(double[] xValues)
        {
            if (xValues.Length != numInput)
                throw new Exception("Bad xValues array length");

            double[] h1Sums = new double[numHidden1]; // hidden nodes sums scratch array
            double[] h2Sums = new double[numHidden2]; // hidden2 nodes sums scratch array
            double[] oSums = new double[numOutput]; // output nodes sums

            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
                this.inputs[i] = xValues[i];

            for (int j = 0; j < numHidden1; ++j)  // compute i-h sum of weights * inputs
                for (int i = 0; i < numInput; ++i)
                    h1Sums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=
            
            for (int i = 0; i < numHidden1; ++i)  // add biases to input-to-hidden sums
                h1Sums[i] += this.h1Biases[i];

            for (int i = 0; i < numHidden1; ++i)   // apply activation
                this.h1Outputs[i] = HyperTanFunction(h1Sums[i]); // hard-coded

            for (int j = 0; j < numHidden2; ++j)  // compute h1-h2 sum of weights * inputs
                for (int i = 0; i < numHidden1; ++i)
                    h2Sums[j] += this.inputs[i] * this.hhWeights[i][j]; // note +=

            for (int i = 0; i < numHidden2; ++i)  // add biases to hidden-to-hidden sums
                h2Sums[i] += this.h2Biases[i];

            for (int i = 0; i < numHidden2; ++i)   // apply activation
                this.h2Outputs[i] = HyperTanFunction(h2Sums[i]); // hard-coded

            for (int j = 0; j < numOutput; ++j)   // compute h-o sum of weights * h2Outputs
                for (int i = 0; i < numHidden2; ++i)
                    oSums[j] += h2Outputs[i] * hoWeights[i][j];

            for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
                oSums[i] += oBiases[i];

            double[] softOut = Softmax(oSums); // softmax activation does all outputs at once for efficiency
            Array.Copy(softOut, outputs, softOut.Length);

            double[] retResult = new double[numOutput]; // could define a GetOutputs method instead
            Array.Copy(this.outputs, retResult, retResult.Length);
            return retResult;
        } // ComputeOutputs

        public static double HyperTanFunction(double x)
        {
            if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
            else if (x > 20.0) return 1.0;
            else return Math.Tanh(x);
        }

        private static double[] Softmax(double[] oSums)
        {
            // Determine max output sum
            // Does all output nodes at once so scale doesn't have to be re-computed each time
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
                if (oSums[i] > max) max = oSums[i];

            // Determine scaling factor -- sum of exp(each val - max)
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;

            return result; // now scaled so that xi sum to 1.0
        }

        // ----------------------------------------------------------------------------------------

        private static void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        }

        // ----------------------------------------------------------------------------------------

    } // Neuronal network code based on the sample provided for 2013 Microsoft Build Conference attendees

    
}
