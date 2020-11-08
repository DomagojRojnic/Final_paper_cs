using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    class Test
    {
        public double calculate_FMeasure(datasetManipulator datasetManipulator)
        {
            double[] expected = datasetManipulator.getExpectedTestDataLabels();
            double[] results = datasetManipulator.getResults();

            int tp = 0, fp = 0, tn = 0, fn = 0;
            double precision, recall;
            for(int i = 0; i < expected.Length; i++)
            {
                if (expected[i] == 1 && results[i] == 1)
                {
                    tp++;
                }
                else if (expected[i] == 1 && results[i] == 0)
                {
                    fn++;
                }
                else if (expected[i] == 0 && results[i] == 1)
                {
                    fp++;
                }
                else if (expected[i] == 0 && results[i] == 0)
                    tn++;
            }
            precision = ((double)tp / ((double)(tp + fp)));
            recall = ((double)tp / ((double)(tp + fn)));
            return ((2 * precision * recall) / (precision + recall));
        }
    }
}
