using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    class Borderline_SMOTE : SMOTE
    {
        
        public Borderline_SMOTE(knn knn) : base(knn) { }

        public override double[][] overSample(double[][] trainingSamples, double[][] minoritySamples, int N, int k)
        {
            return calculateDangerSubset(trainingSamples, minoritySamples, N, k);
        }

        private double[][] calculateDangerSubset(double[][] trainingSamples, double[][] minoritySamples, int N, int k)
        {
            int newIndex = 0;
            int numberOfNeighbors = 2 * k + 1;
            int numberOfMajorityNeighbors;
            int dangerSubsetSize = 0;

            for (int i = 0; i < minoritySamples.Length; i++)
            {
                numberOfMajorityNeighbors = 0;
                double[][] nnarray = this.knn.findKNearestNeighbors(trainingSamples, minoritySamples[i], numberOfNeighbors);
                for (int j = 0; j < nnarray.Length; j++)
                {
                    if (nnarray[j][nnarray[0].Length - 1] == 0)
                        numberOfMajorityNeighbors++;
                }
                if (numberOfMajorityNeighbors >= (numberOfNeighbors / 2) && numberOfMajorityNeighbors < numberOfNeighbors)
                {
                    dangerSubsetSize++;
                }
            }
            double newN = ((trainingSamples.Length - minoritySamples.Length - dangerSubsetSize) / dangerSubsetSize);
            N = (int)Math.Round(newN);

            double[][] dangerSubset = new double[dangerSubsetSize][];
            for (int i = 0; i < dangerSubsetSize; i++)
            {
                dangerSubset[i] = new double[trainingSamples[0].Length];
            }

            for (int i = 0; i < minoritySamples.Length; i++) 
            {
                numberOfMajorityNeighbors = 0;
                double[][] nnarray = this.knn.findKNearestNeighbors(trainingSamples, minoritySamples[i], numberOfNeighbors);
                for (int j = 0; j < nnarray.Length; j++)
                {
                    if (nnarray[j][nnarray[0].Length - 1] == 0)
                        numberOfMajorityNeighbors++;
                }
                if (numberOfMajorityNeighbors >= (numberOfNeighbors / 2) && numberOfMajorityNeighbors < numberOfNeighbors)
                {
                    dangerSubset[newIndex] = minoritySamples[i];
                    newIndex++;
                }
            }
            return base.overSample(trainingSamples, dangerSubset, (N * 100), k);
        }
    }
}
