using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    class datasetManipulator
    {
        double[][] data;

        double[][] majoritySet;
        double[][] minoritySet;

        double[][] minorityTrainingSet;
        double[][] minorityTestSet;

        double[][] majorityTrainingSet;
        double[][] majorityTestSet;

        double[][] trainData;
        double[][] testData;

        IOverSamplingAlgorithm overSamplingAlgorithm;
        double imbalanceRatio;
        knn knn;
        int k;
        int N;

        public datasetManipulator(Dataset dataset, IOverSamplingAlgorithm overSamplingAlgorithm, knn knn, int k, int N)
        {
            this.overSamplingAlgorithm = overSamplingAlgorithm;
            this.knn = knn;
            this.k = k;
            this.N = N;
            data = ConvertData(dataset);
            normalizeData();
            splitDataset();
            shuffleSubset(trainData);
            shuffleSubset(testData);
            
            if(overSamplingAlgorithm.GetType() != typeof(NoOverSampling))
            {
                double[][] synthetic = this.overSamplingAlgorithm.overSample(trainData, minorityTrainingSet, this.N, this.k);
                composeNewTrainingSet(synthetic);
                shuffleSubset(trainData);
            }
        }
        private double[][] ConvertData(Dataset dataset)
        {
            return dataset.GetData().Select(a => a.ToArray()).ToArray();
        }

        private void normalizeData()
        {
            double max, min;
            transposeData();
            for(int i = 0; i < data.Length - 1; i++)
            {
                max = data[i].Max();
                min = data[i].Min();
                for(int j = 0; j < data[i].Length; j++)
                {
                    if(data[i][j] != 0)
                        data[i][j] = (data[i][j] - min) / (max - min);
                }
            }
            transposeData();
        }

        private void transposeData()
        {
            int rows = data.Length;
            int columns = data[0].Length;

            double[][] result = new double[columns][];
            for (int i = 0; i < columns; i++)
                result[i] = new double[rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[j][i] = data[i][j];
                }
            }
            data = result;
        }
        private void splitDataset()
        {
            splitMajorityMinority();
            makeTrainTestSubsets();
        }
        private void splitMajorityMinority()
        {
            int majorityCounter = 0;
            int minorityCounter = 0;
            int majoritySize = 0;
            int minoritySize = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i][data[0].Length - 1] == 0)
                    majorityCounter++;

            }
            minorityCounter = (int)(data.Length - majorityCounter);

            this.imbalanceRatio = (majorityCounter / minorityCounter);
            this.imbalanceRatio = Math.Round(imbalanceRatio);

            majoritySet = new double[majorityCounter][];
            for (int i = 0; i < majorityCounter; i++)
                majoritySet[i] = new double[data[0].Length];
            minoritySet = new double[minorityCounter][];
            for (int i = 0; i < minorityCounter; i++)
                minoritySet[i] = new double[data[0].Length];

            minorityTrainingSet = new double[(int)(0.8 * minorityCounter)][];
            majorityTrainingSet = new double[(int)(0.8 * majorityCounter)][];
            minorityTestSet = new double[minorityCounter - (int)(0.8 * minorityCounter)][];
            majorityTestSet = new double[majorityCounter - (int)(0.8 * majorityCounter)][];

            for (int i = 0; i < (int)(0.8 * majorityCounter); i++)
            {
                majorityTrainingSet[i] = new double[data[0].Length];
            }
            for (int i = 0; i < (int)(0.8 * minorityCounter); i++)
            {
                minorityTrainingSet[i] = new double[data[0].Length];

            }
            for (int i = 0; i < majorityCounter - (int)(0.8 * majorityCounter); i++)
            {

                majorityTestSet[i] = new double[data[0].Length];
            }
            for (int i = 0; i < minorityCounter - (int)(0.8 * minorityCounter); i++)
            {

                minorityTestSet[i] = new double[data[0].Length];
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i][data[0].Length - 1] == 1)
                    minoritySet[minoritySize++] = data[i];
                else
                    majoritySet[majoritySize++] = data[i];
            }
        }

        private void makeTrainTestSubsets()
        {
            int majorityTrainingSize = 0;
            int majorityTestSize = 0;
            Random random = new Random();
            for (int i = 0; i < majoritySet.Length; i++)
            {
                if (majorityTrainingSize == (int)(0.8 * majoritySet.Length))
                    majorityTestSet[majorityTestSize++] = majoritySet[i];
                else if (majorityTestSize == majoritySet.Length - (int)(0.8 * majoritySet.Length))
                    majorityTrainingSet[majorityTrainingSize++] = majoritySet[i];
                else if (random.NextDouble() < 0.8)
                {
                    majorityTrainingSet[majorityTrainingSize++] = majoritySet[i];
                }
                else
                    majorityTestSet[majorityTestSize++] = majoritySet[i];
            }

            int minorityTrainingSize = 0;
            int minorityTestSize = 0;
            for (int i = 0; i < minoritySet.Length; i++)
            {
                if (minorityTrainingSize == (int)(0.8 * minoritySet.Length))
                    minorityTestSet[minorityTestSize++] = minoritySet[i];
                else if (minorityTestSize == minoritySet.Length - (int)(0.8 * minoritySet.Length))
                    minorityTrainingSet[minorityTrainingSize++] = minoritySet[i];
                else if (random.NextDouble() < 0.8)
                {
                    minorityTrainingSet[minorityTrainingSize++] = minoritySet[i];
                }
                else
                    minorityTestSet[minorityTestSize++] = minoritySet[i];
            }

            trainData = new double[(minorityTrainingSet.Length + majorityTrainingSet.Length)][];
            for (int i = 0; i < trainData.Length; i++)
            {
                trainData[i] = new double[data[0].Length];
            }
            testData = new double[(minorityTestSet.Length+majorityTestSet.Length)][];
            for (int i = 0; i < testData.Length; i++)
            {
                testData[i] = new double[data[0].Length];
            }

            for (int i = 0; i < minorityTrainingSet.Length; i++)
            {
                trainData[i] = minorityTrainingSet[i];
            }
            for (int i = minorityTrainingSet.Length; i < (minorityTrainingSet.Length+majorityTrainingSet.Length); i++)
            {
                trainData[i] = majorityTrainingSet[i-minorityTrainingSet.Length];
            }

            for (int i = 0; i < minorityTestSet.Length; i++)
            {
                testData[i] = minorityTestSet[i];
            }
            for (int i = minorityTestSet.Length; i < (minorityTestSet.Length + majorityTestSet.Length); i++)
            {
                testData[i] = majorityTestSet[i-minorityTestSet.Length];
            }
        }
        
        public void composeNewTrainingSet(double[][] syntheticSubset)
        {
            trainData = new double[majorityTrainingSet.Length + syntheticSubset.Length + minorityTrainingSet.Length][];
            for(int i = 0; i < trainData.Length; i++)
            {
                trainData[i] = new double[data[0].Length];
            }

            for(int i = 0; i < majorityTrainingSet.Length; i++)
            {
                trainData[i] = majorityTrainingSet[i];
            }
            for(int i = majorityTrainingSet.Length; i < (majorityTrainingSet.Length + syntheticSubset.Length); i++)
            {
                trainData[i] = syntheticSubset[i - majorityTrainingSet.Length];
            }
            for(int i = (majorityTrainingSet.Length + syntheticSubset.Length); i< (majorityTrainingSet.Length + syntheticSubset.Length + minorityTrainingSet.Length); i++)
            {
                trainData[i] = minorityTrainingSet[i - (majorityTrainingSet.Length + syntheticSubset.Length)];
            }
        }

        public void shuffleSubset(double[][] subset)
        {
            Random random = new Random();

            for(int i=0; i < subset.Length; i++)
            {
                int j = random.Next(0, i + 1);
                if(i != j)
                {
                    double[] temp = subset[i];
                    subset[i] = subset[j];
                    subset[j] = temp;
                }
            }
        }

        public double[] getExpectedTestDataLabels()
        {
            double[] expected = new double[testData.Length];
            for(int i = 0; i < testData.Length; i++)
            {
                expected[i] = testData[i][testData[0].Length - 1];
            }
            return expected;
        }

        public double[] getResults()
        {
            int majority, minority;
            double[] results = new double[testData.Length];

            for(int i = 0; i < testData.Length; i++)
            {
                majority = 0;
                minority = 0;
                double[][] nnarray = this.knn.findKNearestTestNeighbors(trainData, testData[i], this.k);
                for(int j = 0; j < nnarray.Length; j++)
                {
                    if (nnarray[j][nnarray[0].Length - 1] == 1)
                        minority++;
                    else
                    {
                        majority++;
                    }
                }
                if (majority > minority)
                {
                    results[i] = 0;
                }
                else
                {
                    results[i] = 1;
                }
            }
            return results;
        }
    }
}
