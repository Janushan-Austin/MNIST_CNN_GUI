using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public struct MaxPoolProps
    {
        public int PoolCols, PoolRows;

        public MaxPoolProps(int poolCols, int poolRows)
        {
            PoolCols = poolCols; PoolRows = poolRows;
        }

        public MaxPoolProps(ConvolutionalLayerProps layerProps)
        {
            PoolCols = layerProps.PoolCols; PoolRows = layerProps.PoolRows;
        }
    }

    public struct MaxPoolIndexPair
    {
        public int col, row;

        public MaxPoolIndexPair(int col_ =0 , int row_ = 0)
        {
            col = col_;
            row = row_;
        }
    }
    public class MaxPool
    {
        public float[][] Pool;
        public MaxPoolIndexPair[][] IndexPairs;

        public MaxPool(MaxPoolProps props)
        {
            Pool = new float[props.PoolRows][];
            IndexPairs = new MaxPoolIndexPair[props.PoolRows][];
            for(int i = 0; i < IndexPairs.Length; i++)
            {
                Pool[i] = new float[props.PoolCols];
                IndexPairs[i] = new MaxPoolIndexPair[props.PoolCols];
                for(int j =0; j < IndexPairs[i].Length; j++)
                {
                    IndexPairs[i][j] = new MaxPoolIndexPair();
                }
            }
        }

    }
}
