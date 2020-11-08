using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    class SMOTE : IOverSamplingAlgorithm
    {
        protected knn knn;

        public SMOTE(knn knn)
        {
            this.knn = knn;
        }
        public virtual double[][] overSample(double[][] trainingSamples, double[][] minoritySamples, int N, int k)
        {
            int newIndex = 0;
            double[][] synthetic = new double[((N/100)) * minoritySamples.Length][];
            for(int i = 0; i < synthetic.Length; i++)
            {
                synthetic[i] = new double[minoritySamples[0].Length];
            }
            for (int i = 0; i < minoritySamples.Length; i++)
            {
                double[][] nnarray = this.knn.findKNearestNeighbors(minoritySamples, minoritySamples[i], k);
                newIndex = populateSyntheticSamples(N, nnarray, minoritySamples[i], k, synthetic, newIndex);
            }
            return synthetic;
        }

        private int populateSyntheticSamples(int N, double[][] nearestNeighborsArray, double[] minoritySample, int k, double[][] synthetic, int newIndex)
        {
            int randomNeighbor;
            double difference, gap;
            N = N / 100;
            Random random = new Random();
            while (N != 0)
            {
                randomNeighbor = random.Next(k);
                for (int i = 0; i < nearestNeighborsArray[0].Length; i++)
                {
                    difference = nearestNeighborsArray[randomNeighbor][i] - minoritySample[i];
                    gap = random.NextDouble();
                    synthetic[newIndex][i] = Math.Round(minoritySample[i] + gap * difference, 6);
                }
                newIndex++;
                N--;
            }
            return newIndex;
        }
    }
}
