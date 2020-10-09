using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public struct ConvolutionalNeuralNetworkProps
    {
        public int InputCols, InputRows, InputSize, ConvolutionalOutputs;
        public int NumberHiddenNodes, NumberOutputs;
        public int NeighborhoodCols, NeighborhoodRows;
        public int PoolCols, PoolRows, PoolSamplingCols, PoolSamplingRows, PoolSize;
        public int NumberFeatures, FeatureRows, FeatureCols;

        public ConvolutionalNeuralNetworkProps(int inputCols, int inputRows, int hiddenNodes_, int numberOutputs_, int numFeatures = 0, int neighborhoodCols_ = 0, int neightborhoodRows_ = 0, int poolSamplingCols_ = 0, int poolSamplingRows_ = 0)
        {
            InputCols = inputCols;
            InputRows = inputRows;
            InputSize = inputCols * inputRows;
            NumberHiddenNodes = hiddenNodes_;
            NumberOutputs = numberOutputs_;
            NumberFeatures = numFeatures;
            NeighborhoodCols = neighborhoodCols_;
            NeighborhoodRows = neightborhoodRows_;
            PoolSamplingCols = poolSamplingCols_;
            PoolSamplingRows = poolSamplingRows_;


            FeatureCols = 1 + InputCols - NeighborhoodCols; FeatureRows = 1 + InputRows - NeighborhoodRows;
            PoolCols = FeatureCols / PoolSamplingCols; PoolRows = FeatureRows / PoolSamplingRows;
            PoolSize = PoolRows * PoolCols;
            ConvolutionalOutputs = PoolSize * NumberFeatures;
        }
    }

    //TODO: Get Batching working
    public class ConvolutionalNeuralNetwork : NeuralNetwork
    {
        ConvolutionalNeuralNetworkProps Props;
        public ConvolutionalNeuralNetwork(ConvolutionalNeuralNetworkProps props,  uint batches = 1) 
            : base() //base(numInputs, numHiddenNodes, numOutputs, batches)
        {
            Props = props;
            Initialize((uint)props.ConvolutionalOutputs, (uint)props.NumberHiddenNodes, (uint)props.NumberOutputs, batches);
            NumInputs = (uint)Props.InputSize;
            InputLayer = Layers[0] = new ConvolutionalLayer(new ConvolutionalLayerProps(props), m_ActivationFunction, batches, false);
            Reset();
            m_ActivationFunction.ChangeFunction(ActivationFunctions.FunctionTypes.Sigmoid);
            m_ActivationFunction.FunctionLearningRate = 0.075f;
        }

        public override bool UploadTrainingSet(IDataSet[] trainingSet)
        {
            if (trainingSet == null || trainingSet[0].ExpectedOutputSet == null || trainingSet[0].InputSet == null ||
               trainingSet[0].InputSet.Length != Props.InputSize || trainingSet[0].ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            TrainingSet = trainingSet;
            TrainingSetSize = (uint)trainingSet.Length;

            return true;
        }

        public override bool UploadTestSet(IDataSet[] testSet)
        {
            if (testSet == null || testSet[0].InputSet == null || testSet[0].ExpectedOutputSet == null ||
               testSet[0].InputSet.Length != Props.InputSize || testSet[0].ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            TestSet = testSet;
            TestSetSize = (uint)testSet.Length;

            return true;
        }
    }
}
