using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    interface IOverSamplingAlgorithm
    {
        double[][] overSample(double[][] trainingSamples, double[][] minoritySamples, int N, int k);
    }
}
