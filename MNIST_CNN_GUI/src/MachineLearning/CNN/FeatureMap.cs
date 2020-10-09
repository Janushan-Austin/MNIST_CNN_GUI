using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public struct FeatureMapProps
    {
        public int NeighborhoodCols, neighborhoodRows;
        public int FeatureRows, FeatureCols;

        public FeatureMapProps(int neighborhoodCols_ = 0, int neighborhoodRows_ = 0)
        {
            NeighborhoodCols = neighborhoodCols_;
            neighborhoodRows = neighborhoodRows_;
            FeatureRows = 0;
            FeatureCols = 0;
        }

        public FeatureMapProps(ConvolutionalLayerProps layerProps)
        {
            NeighborhoodCols = layerProps.NeighborhoodCols;
            neighborhoodRows = layerProps.NeighborhoodRows;
            FeatureRows = layerProps.FeatureRows;
            FeatureCols = layerProps.FeatureCols;
        }
    }

    public class FeatureMap
    {
        FeatureMapProps Props;
        public float[][] Features;
        public float[] Weights, OldWeights;

        public FeatureMap(FeatureMapProps props)
        {
            Props = props;
            Weights = new float[props.NeighborhoodCols * props.neighborhoodRows + 1];
            OldWeights = new float[Weights.Length];

            Features = new float[props.FeatureRows][];
            for(int i =0; i < Features.Length; i++)
            {
                Features[i] = new float[props.FeatureCols];
            }
        }

        public void ResetWeights()
        {
            for (uint i = 0; i < Weights.Length; i++)
            {
                Weights[i] = Rand.NextFloat() - 0.5f;
            }
        }
    }
}
