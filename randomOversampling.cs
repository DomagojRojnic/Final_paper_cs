using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    class randomOversampling : IOverSamplingAlgorithm
    {
        private static randomOversampling instance;
        public static randomOversampling GetInstance()
        {
            if (instance == null)
            {
                instance = new randomOversampling();
            }
            return instance;
        }
        public double[][] overSample(double[][] trainingSamples, double[][] minoritySamples, int N, int k)
        {
            double[][] synthetic = new double[(N / 100) * minoritySamples.Length][];
            for(int i = 0; i < synthetic.Length; i++)
            {
                synthetic[i] = new double[minoritySamples[0].Length];
            }

            int newIndex = 0;
            Random random = new Random();
            while (newIndex != synthetic.Length)
            {
                synthetic[newIndex] = minoritySamples[random.Next(0, minoritySamples.Length)];
                newIndex++;
            }
            return synthetic;
        }
    }
}
