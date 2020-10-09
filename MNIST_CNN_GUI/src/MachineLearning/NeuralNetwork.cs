using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class NeuralNetwork
    {
        protected Layer[] Layers;
        protected Layer InputLayer;
        protected Layer HiddenLayer;
        protected Layer OutputLayer;
        protected uint NumInputs, NumHiddenNodes, NumOutputs;

        public uint NumEpochs = 1;
        public float TotalEpochs { get; protected set; }
        public uint CompletedEpochs { get; protected set; }

        protected bool LastPropagateWasATest;
        protected bool InvalidatedNetwork;

        public ActivationFunctions m_ActivationFunction;
        protected ConfusionMatrix ConfusionMatrix;

        #region Mini Batch properties
        private uint TotalBatches;
        public uint Batches { get { return TotalBatches; } private set { if (value != 0) { UpdateBatches(value); } } }
        public int CompletedBatches { get; private set; }
        protected bool IncrementBatch()
        {
            if (CompletedBatches < Batches - 1)
            {
                CompletedBatches++;
                return true;
            }
            return false;
        }
        protected void ResetBatch()
        {
            CompletedBatches = 0;
        }
        public bool ChangeBatches(uint batches)
        {
            if (batches == 0 || batches == Batches)
            {
                return false;
            }

            UpdateBatches(batches);
            return true;
        }

        protected void UpdateBatches(uint batches)
        {
            if (CompletedBatches > 0)
            {
                BackPropagate();
            }
            ResetBatch();
            TotalBatches = batches;

            for (uint i = 0; i < Layers.Length; i++)
            {
                Layers[i].Batches = batches;
            }
        }
        #endregion

        public NeuralNetwork()
        {

        }

        public NeuralNetwork(uint numInputs, uint numHiddenNodes, uint numOutputs, uint batches = 1)
        {
            Initialize(numInputs, numHiddenNodes, numOutputs, batches);
        }

        #region Network Initializations

        protected virtual void Initialize(uint numInputs, uint numHiddenNodes, uint numOutputs, uint batches = 1)
        {
            NumInputs = numInputs;
            NumHiddenNodes = numHiddenNodes;
            NumOutputs = numOutputs;

            m_ActivationFunction = new ActivationFunctions();
            Layers = new Layer[3];

            if (batches < 1)
            {
                batches = 1;
            }
            TotalBatches = batches;
            ResetBatch();

            InputLayer = Layers[0] = new Layer(numInputs, 0, m_ActivationFunction, batches);
            HiddenLayer = Layers[1] = new Layer(numHiddenNodes, numInputs, m_ActivationFunction, batches);
            OutputLayer = Layers[Layers.Length - 1] = new Layer(numOutputs, numHiddenNodes, m_ActivationFunction, batches, true);

            ConfusionMatrix = new ConfusionMatrix(numOutputs);

            Reset();
        }
        public virtual void Reset()
        {
            InvalidatedNetwork = false;
            ConfusionMatrix.ResetMatrix();
            ResetWeights();
            ResetBatch();
            CompletedEpochs = 0;
            TotalEpochs = 0;
        }

        public virtual void ResetWeights()
        {
            for (uint i = 0; i < Layers.Length; i++)
            {
                Layers[i].ResetWeights();
            }
        }

        public void UpdateLayerNumberPerceptrons(uint layer, uint numberPerceptrons)
        {
            if (layer > Layers.Length)
            {
                return;
            }

            InvalidatedNetwork = true;
            Layers[layer].UpdateNumberPerceptrons(numberPerceptrons);
            if (layer + 1 < Layers.Length)
            {
                if (layer == 0)
                {
                    NumInputs = numberPerceptrons;
                    TrainingSet = null;
                    TrainingSetSize = 0;
                    TestSet = null;
                    TestSetSize = 0;
                }
                Layers[layer + 1].UpdateNumberInputs(numberPerceptrons);
            }
            else
            {
                NumOutputs = numberPerceptrons;
                ConfusionMatrix = new ConfusionMatrix(NumOutputs);
                TrainingSet = null;
                TrainingSetSize = 0;
                TestSet = null;
                TestSetSize = 0;
            }

        }

        public bool ChangeLayerActivationFunction(int layer, ActivationFunctions.FunctionTypes funcType)
        {
            if (layer < 0 || layer >= Layers.Length)
            {
                return false;
            }
            return Layers[layer].ChangeActivationFunction(funcType);
        }

        public bool ChangeLayerActivationFunction(ActivationFunctions.FunctionTypes funcType)
        {
            bool allValid = true;
            for (int i = 0; i < Layers.Length; i++)
            {
                if (Layers[i].ChangeActivationFunction(funcType) == false)
                {
                    allValid = false;
                }
            }
            return allValid;
        }

        public void ChangeLayerLearningRate(int layer, float learningRate)
        {
            Layers[layer].ChangeLearningRate(learningRate);
        }
        #endregion

        #region Training and Testing

        protected IDataSet[] TrainingSet;
        protected IDataSet[] TestSet;
        public uint TrainingSetSize;
        public uint TestSetSize;

        public float TrainingSensitivity { get; protected set; }
        public float TestingSensitivity { get; protected set; }
        public float TestingLoss { get; protected set; }

        public virtual bool UploadTrainingSet(IDataSet[] trainingSet)
        {
            if (trainingSet == null || trainingSet[0].ExpectedOutputSet == null || trainingSet[0].InputSet == null ||
               trainingSet[0].InputSet.Length != NumInputs || trainingSet[0].ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            TrainingSet = trainingSet;
            TrainingSetSize = (uint)trainingSet.Length;

            return true;
        }

        public virtual bool UploadTestSet(IDataSet[] testSet)
        {
            if (testSet == null || testSet[0].InputSet == null || testSet[0].ExpectedOutputSet == null ||
               testSet[0].InputSet.Length != NumInputs || testSet[0].ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            TestSet = testSet;
            TestSetSize = (uint)testSet.Length;

            return true;
        }

        public virtual void Train(Func<int, bool, bool> progressFunc = null, bool continueLastTrainingSet = false)
        {
            if (TrainingSet == null)
            {
                return;
            }

            float prevTrainingSensitivity = TrainingSensitivity;
            float epochCompletionPerPropagation = 1.0f / TrainingSetSize;
            uint startSet = continueLastTrainingSet == true ? 
                (uint)((TotalEpochs - (uint)TotalEpochs) * TrainingSetSize) : 0;

            float deltaSetPercent = 1.0f / (NumEpochs * TrainingSetSize) * 100.0f;
            float percent = startSet * deltaSetPercent;
            bool earlyExit = false;

            for (uint epoch = 0; epoch < NumEpochs; epoch++)
            {
                ConfusionMatrix.ResetMatrix();
                for (uint set = startSet; set < TrainingSetSize; set++, percent += deltaSetPercent)
                {
                    if (progressFunc != null && progressFunc((int)percent, false) == false)
                    {
                        earlyExit = true;
                        break;
                    }
                    Propagate(TrainingSet[set], testing: false);
                    EvaluateOutput();
                    TotalEpochs += epochCompletionPerPropagation;
                }
                startSet = 0;
                CompletedEpochs++;

                if (CompletedBatches > 0)
                {
                    BackPropagate();
                }

                TrainingSensitivity = Sensitivity();

                Test();

                while (TotalEpochs - CompletedEpochs > 1.0f)
                {
                    CompletedEpochs++;
                }
                if (earlyExit == true)
                {
                    return;
                }

                if (progressFunc != null && progressFunc((int)percent, true) == false)
                {
                    return;
                }
            }
        }

        public virtual void TestTrainingSet()
        {
            if (TrainingSet == null)
            {
                return;
            }

            ConfusionMatrix.ResetMatrix();

            for (uint set = 0; set < TrainingSetSize; set++)
            {
                Propagate(TrainingSet[set], testing: true);
                EvaluateOutput();
            }

            TrainingSensitivity = Sensitivity();
        }

        public virtual void Test(bool ignoreLoss = false)
        {
            if (TestSet == null)
            {
                return;
            }
            
            float prevTestSensitivity = TestingSensitivity;
            float prevLoss = TestingLoss;
            ConfusionMatrix.ResetMatrix();
            TestingLoss = 0;

            for (uint set = 0; set < TestSetSize; set++)
            {
                Propagate(TestSet[set], testing: true);
                EvaluateOutput();
                TestingLoss += Loss();
            }

            TestingLoss /= TestSetSize;
            TestingSensitivity = Sensitivity();

            if (ignoreLoss == false && prevLoss - TestingLoss < 1e-4)
            {
                m_ActivationFunction.FunctionLearningRate /= 2;
            }
        }



        #endregion

        #region Propagation
        public bool Propagate(IDataSet data, bool testing = false)
        {
            if (data.InputSet.Length != NumInputs || data.ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            if (InvalidatedNetwork == true)
            {
                Reset();
            }

            if (testing == true && CompletedBatches > 0)
            {
                BackPropagate();
                CompletedBatches = 0;
            }

            InputLayer.UploadInputs(data.InputSet, CompletedBatches);
            OutputLayer.UploadExpectedOutputs(data.ExpectedOutputSet, CompletedBatches);

            ForwardPropagate();
            if (testing == false)
            {
                if (IncrementBatch() == false)
                {
                    CompletedBatches = (int)Batches;
                    BackPropagate();
                }
            }
            LastPropagateWasATest = testing;

            return true;
        }

        protected virtual void ForwardPropagate()
        {
            Layer currLayer = Layers[0];
            Layer prevLayer = null;
            for (uint L = 0; L < Layers.Length; L++)
            {
                currLayer = Layers[L];
                currLayer.ForwardPropagate(prevLayer, CompletedBatches);
                prevLayer = currLayer;
            }
        }

        protected virtual void BackPropagate()
        {
            Layer nextLayer = null;
            Layer currLayer = Layers[Layers.Length - 1];
            Layer prevLayer = Layers[Layers.Length - 2];
            for (int layer = Layers.Length - 1; layer > 1; layer--)
            {
                currLayer.BackPropagate(prevLayer, nextLayer, CompletedBatches);
                nextLayer = currLayer;
                currLayer = prevLayer;
                prevLayer = Layers[layer - 2];
            }

            currLayer.BackPropagate(prevLayer, nextLayer, CompletedBatches);
            prevLayer.BackPropagate(null, currLayer, CompletedBatches);
            ResetBatch();
        }
        #endregion


        #region Network Results and Statistics
        public float Loss()
        {
            float loss = 0;
            float[] errors = Errors();

            foreach (float error in errors)
            {
                loss += error * error;
            }

            return loss / errors.Length;
        }

        public float[] Outputs()
        {
            int currentBatch = (CompletedBatches != 0 ? CompletedBatches : (int)Batches) - 1;
            if (LastPropagateWasATest == true)
            {
                currentBatch = 0;
            }

            float[] outputs = new float[OutputLayer.Perceptrons.Length];

            for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            {
                outputs[i] = OutputLayer.Perceptrons[i].Outputs[currentBatch];
            }

            return outputs;
        }

        public float[] ExpectedOutputs()
        {
            int currentBatch = (CompletedBatches != 0 ? CompletedBatches : (int)Batches) - 1;
            if (LastPropagateWasATest == true)
            {
                currentBatch = 0;
            }

            float[] outputs = new float[OutputLayer.Perceptrons.Length];

            for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            {
                outputs[i] = OutputLayer.Perceptrons[i].ExpectedOutputs[currentBatch];
            }

            return outputs;
        }

        public float[] Errors()
        {
            int currentBatch = (CompletedBatches != 0 ? CompletedBatches : (int)Batches) - 1;
            if (LastPropagateWasATest == true)
            {
                currentBatch = 0;
            }
            float[] errors = new float[OutputLayer.Perceptrons.Length];

            for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            {
                errors[i] = OutputLayer.Perceptrons[i].Outputs[currentBatch] - OutputLayer.Perceptrons[i].ExpectedOutputs[currentBatch];
            }

            return errors;
        }
        public virtual void EvaluateOutput()
        {
            float[] expectedOutputs = ExpectedOutputs();
            uint actualIndex = 0;
            float maxActualValue = expectedOutputs[0];

            float[] outputs = Outputs();
            uint PredictedIndex = 0;
            float maxPredictedValue = outputs[0];
            for (uint i = 1; i < outputs.Length; i++)
            {
                if (outputs[i] > maxPredictedValue)
                {
                    maxPredictedValue = outputs[i];
                    PredictedIndex = i;
                }

                if (expectedOutputs[i] > maxActualValue)
                {
                    maxActualValue = expectedOutputs[i];
                    actualIndex = i;
                }
            }

            ConfusionMatrix.AddToIndex(actualIndex, PredictedIndex);
        }

        #region Confusion Matrix Results
        public virtual uint TruePositives(int classType = -1)
        {
            return ConfusionMatrix.GetTruePositives(classType);
        }

        public virtual uint TrueNegatives(int classType = -1)
        {
            return ConfusionMatrix.GetTrueNegatives(classType);
        }

        public virtual uint FalsePositives(int classType = -1)
        {
            return ConfusionMatrix.GetFalsePositives(classType);
        }

        public virtual uint FalseNegatives(int classType = -1)
        {
            return ConfusionMatrix.GetFalseNegatives(classType);
        }

        public virtual float ClassificationAccuracy(int classType = -1)
        {
            return ConfusionMatrix.ClassificationAccuracy(classType) * 100;
        }

        public virtual float ClassificationError(int classType = -1)
        {
            return (1.0f - ConfusionMatrix.ClassificationAccuracy(classType)) * 100;
        }

        public virtual float Precision(int classType = -1)
        {
            return ConfusionMatrix.Precision(classType) * 100;
        }

        public virtual float Specificity(int classType = -1)
        {
            return ConfusionMatrix.Specificity(classType) * 100;
        }

        public virtual float Sensitivity(int classType = -1)
        {
            return ConfusionMatrix.Sensitivity(classType) * 100;
        }
        #endregion

        #endregion
    }
}
