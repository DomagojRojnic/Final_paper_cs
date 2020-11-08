using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZavrsniRadRojnic
{
    class knn
    {
        public double[][] findKNearestNeighbors(double[][] trainSamples, double[] testSample, int k)
        {
            double[][] trainSamplesCopy = trainSamples;
            quicksort(trainSamplesCopy, testSample, 0, trainSamplesCopy.Length - 1);

            double[][] neighbors = new double[k][];
            for(int i = 0; i < k; i++)
            {
                neighbors[i] = new double[trainSamplesCopy[0].Length];
                neighbors[i] = trainSamplesCopy[i+1];
            }
            return neighbors;
        }

        public double[][] findKNearestTestNeighbors(double[][] trainSamples, double[] testSample, int k)
        {

            double[][] trainSamplesCopy = trainSamples;
            quicksort(trainSamplesCopy, testSample, 0, trainSamplesCopy.Length - 1);
            
            double[][] neighbors = new double[k][];
            for (int i = 0; i < k; i++)
            {
                neighbors[i] = new double[trainSamplesCopy[0].Length];
                neighbors[i] = trainSamplesCopy[i];
            }
            return neighbors;
        }

        private double calculateEuclideanDistance(double[] x, double[] y)
        {
            double sum = 0;
            for (int i=0; i< x.Length - 1; i++)
            {
                sum += Math.Pow(y[i] - x[i], 2);
            }
            return Math.Sqrt(sum);
        }

        private int partition(double[][] train, double[] test, int low, int high)
        {
            double pivot = calculateEuclideanDistance(train[high], test);
            int i = (low - 1);
            for(int j = low; j < high; j++)
            {
                if(calculateEuclideanDistance(train[j], test) < pivot)
                {
                    i++;
                    double[] temp = train[i];
                    train[i] = train[j];
                    train[j] = temp;
                }
            }

            double[] temp1 = train[i + 1];
            train[i + 1] = train[high];
            train[high] = temp1;

            return i + 1;
        }

        private void quicksort(double[][] train, double[] test, int low, int high)
        {
            if (low < high)
            {
                int pi = partition(train, test, low, high);
                quicksort(train, test, low, pi - 1);
                quicksort(train, test, pi + 1, high);
            }
        }
    }
}
