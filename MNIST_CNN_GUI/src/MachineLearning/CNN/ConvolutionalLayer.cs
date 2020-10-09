using MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public struct ConvolutionalLayerProps
    {
        public int InputCols, InputRows, ConvolutionalOutputs;
        public int NeighborhoodCols, NeighborhoodRows;
        public int PoolCols, PoolRows, PoolSamplingCols, PoolSamplingRows, PoolSize;
        public int NumberFeatures, FeatureCols, FeatureRows;

        public ConvolutionalLayerProps(int inputCols, int inputRows, int numFeatures = 0, int neighborhoodCols_ = 0, int neightborhoodRows_ = 0, int poolX_ = 0, int poolY_ = 0)
        {
            InputCols = inputCols;
            InputRows = inputRows;
            NumberFeatures = numFeatures;
            NeighborhoodCols = neighborhoodCols_;
            NeighborhoodRows = neightborhoodRows_;
            PoolSamplingCols = poolX_;
            PoolSamplingRows = poolY_;

            FeatureCols = 1 + InputCols - NeighborhoodCols; FeatureRows = 1 + InputRows - NeighborhoodRows;
            PoolCols = FeatureCols / PoolSamplingCols; PoolRows = FeatureRows / PoolSamplingRows;
            PoolSize = PoolRows * PoolCols;
            ConvolutionalOutputs = PoolSize * NumberFeatures;
        }

        public ConvolutionalLayerProps(ConvolutionalNeuralNetworkProps networkProps)
        {
            InputCols = networkProps.InputCols; InputRows = networkProps.InputRows; NumberFeatures = networkProps.NumberFeatures;
            NeighborhoodCols = networkProps.NeighborhoodCols; NeighborhoodRows = networkProps.NeighborhoodRows;
            PoolSamplingCols = networkProps.PoolSamplingCols; PoolSamplingRows = networkProps.PoolSamplingRows;
            ConvolutionalOutputs = networkProps.ConvolutionalOutputs;
            FeatureCols = networkProps.FeatureCols; FeatureRows = networkProps.FeatureRows;
            PoolCols = networkProps.PoolCols; PoolRows = networkProps.PoolRows; PoolSize = networkProps.PoolSize;
        }
    }
    public class ConvolutionalLayer : Layer
    {
        ConvolutionalLayerProps Props;
        float[][] InputSegment;
        FeatureMap[] FeatureMaps;
        MaxPool[] MaxPools;

        //TODO figure out batching for a CNN
        public ConvolutionalLayer(ConvolutionalLayerProps props, ActivationFunctions activationFunction, uint batches = 1, bool outputLayer = false)
            : base((uint)props.ConvolutionalOutputs, 0, activationFunction, batches, outputLayer)
        {
            Props = props;
            InputSegment = new float[props.InputRows][];
            for (int i = 0; i < InputSegment.Length; i++)
            {
                InputSegment[i] = new float[props.InputCols];
            }

            FeatureMapProps featureProps = new FeatureMapProps(props);
            FeatureMaps = new FeatureMap[props.NumberFeatures];
            for (int f = 0; f < FeatureMaps.Length; f++)
            {
                FeatureMaps[f] = new FeatureMap(featureProps);
            }

            MaxPoolProps poolProps = new MaxPoolProps(props);
            MaxPools = new MaxPool[props.NumberFeatures];
            for (int p = 0; p < FeatureMaps.Length; p++)
            {
                MaxPools[p] = new MaxPool(poolProps);
            }
        }

        public override void ResetWeights()
        {
            for (int i = 0; i < FeatureMaps.Length; i++)
            {
                FeatureMaps[i].ResetWeights();
            }
        }

        public override void UploadInputs(float[] inputs, int completedBatches)
        {
            for (int r = 0; r < InputSegment.Length; r++)
            {
                for (int c = 0; c < InputSegment[0].Length; c++)
                {
                    InputSegment[r][c] = inputs[r * InputSegment[0].Length + c];
                }
            }
        }

        public override void ForwardPropagate(Layer prevLayer, int completedBatches)
        {
            if (prevLayer == null)
            {
                ForwardPropagateInputLayer(completedBatches);
            }
        }

        public override void BackPropagate(Layer prevLayer, Layer forwardLayer, int completedBatches)
        {
            if(prevLayer == null)
            {
                int currentOutputIndex = 0;
                completedBatches--;
                for(int feature = 0; feature < Props.NumberFeatures; feature++)
                {
                    for(int w = 0; w < FeatureMaps[feature].Weights.Length; w++)
                    {
                        FeatureMaps[feature].OldWeights[w] = FeatureMaps[feature].Weights[w];
                    }
                    for(int poolRow = 0; poolRow < Props.PoolRows; poolRow++)
                    {
                        for(int poolCol = 0; poolCol < Props.PoolCols; poolCol++, currentOutputIndex++)
                        {
                            //calculate the back propagation summation for max pool index (poolRow, poolCol)
                            float backPropSummation = 0;
                            for(int p = 0; p < forwardLayer.Perceptrons.Length; p++)
                            {
                                Perceptron perceptron = forwardLayer.Perceptrons[p];
                                backPropSummation += perceptron.BackPropigationDerivatives[completedBatches] * perceptron.OldWeights[currentOutputIndex];
                            }

                            //now time to go through and update the weights based on the current pool element
                            // back tracking through the feature map and into the Input segment to update weights respectively
                            int yStart = poolRow * Props.PoolSamplingRows + MaxPools[feature].IndexPairs[poolRow][poolCol].row;
                            int yEnd = yStart + Props.NeighborhoodRows;
                            int xStart = poolCol * Props.PoolSamplingCols + MaxPools[feature].IndexPairs[poolRow][poolCol].col;
                            int xEnd = xStart + Props.NeighborhoodCols;

                            int weightIndex = 0;
                            for (int y = yStart; y < yEnd; y++)
                            {
                                for(int x = xStart; x < xEnd; x++, weightIndex++)
                                {
                                    FeatureMaps[feature].Weights[weightIndex] -= 
                                        ActivationFunction.FunctionLearningRate * backPropSummation * InputSegment[y][x];
                                }
                            }

                            FeatureMaps[feature].Weights[weightIndex] -=
                                        ActivationFunction.FunctionLearningRate * backPropSummation;
                        }
                    }
                }
            }
        }

        //TODO: implement feature map -> max pool simaltaneously
        protected override void ForwardPropagateInputLayer(int completedBatch)
        {
            FeatureMap featureMap = null;
            MaxPool maxPool = null;
            for (int feature = 0; feature < Props.NumberFeatures; feature++)
            {
                featureMap = FeatureMaps[feature];
                int outputPoolOffset = feature * Props.PoolSize;

                for (int featureRow = 0; featureRow < Props.FeatureRows; featureRow++)
                {
                    for (int featureCol = 0; featureCol < Props.FeatureCols; featureCol++)
                    {
                        featureMap.Features[featureRow][featureCol] = featureMap.Weights[featureMap.Weights.Length -1];
                        int weightIndex = 0;
                        for (int y = 0; y < Props.NeighborhoodRows; y++)
                        {
                            for (int x = 0; x < Props.NeighborhoodCols; x++, weightIndex++)
                            {
                                featureMap.Features[featureRow][featureCol] += 
                                    InputSegment[y + featureRow][x + featureCol] * featureMap.Weights[weightIndex];

                            }
                        }
                    }
                }

                maxPool = MaxPools[feature];
                MaxPoolIndexPair pair = new MaxPoolIndexPair();
                float max;
                int poolIndex = outputPoolOffset;
                for (int poolRow =0; poolRow < Props.PoolRows; poolRow++)
                {
                    int featureRow = poolRow * Props.PoolSamplingRows;
                    for(int poolCol = 0; poolCol < Props.PoolCols; poolCol++, poolIndex++)
                    {
                        int featureCol = poolCol * Props.PoolSamplingCols;
                        max = featureMap.Features[featureRow][featureCol];
                        for(int y =0; y < Props.PoolSamplingRows; y++)
                        {
                            for(int x = 0; x < Props.PoolSamplingCols; x++)
                            {
                                if(featureMap.Features[featureRow + y][featureCol + x] > max)
                                {
                                    max = featureMap.Features[featureRow + y][featureCol + x];
                                    pair.col = x; pair.row = y;
                                }
                            }
                        }

                        maxPool.Pool[poolRow][poolCol] = max;
                        maxPool.IndexPairs[poolRow][poolCol] = pair;

                        Perceptrons[poolIndex].Outputs[completedBatch] = max;
                    }
                }
            }
        }
    }
}
