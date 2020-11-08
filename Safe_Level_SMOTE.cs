using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavrsniRadRojnic
{
    class Safe_Level_SMOTE : SMOTE
    {
        public Safe_Level_SMOTE(knn knn) : base(knn) { }
        public override double[][] overSample(double[][] trainingSamples, double[][] minoritySamples, int N, int k)
        {
            Random random = new Random();
            int rand, slp, sln, newIndex = 0;
            double sl_ratio, gap = 0, difference;

            double[][] synthetic = new double [(N/100) * minoritySamples.Length][];
            for(int i = 0; i < synthetic.Length; i++)
            {
                synthetic[i] = new double[minoritySamples[0].Length];
            }

        loop:
            for (int i = 0; i < minoritySamples.Length; i++)
            {
                slp = 0;
                sln = 0;
                double[][] nnarray = this.knn.findKNearestNeighbors(trainingSamples, minoritySamples[i], k);
                rand = random.Next(0, k);
                for (int j = 0; j < k; j++)
                {
                    if (nnarray[j][nnarray[0].Length - 1] == 1)
                        slp++;
                }
                if (slp == 0)
                    continue;
                else
                {
                    while (nnarray[rand][nnarray[0].Length - 1] != 1)
                    {
                        rand = random.Next(0, k);
                    }

                    double[][] randNeighbor_nnarray = this.knn.findKNearestNeighbors(trainingSamples, nnarray[rand], k);
                    for (int j = 0; j < k; j++)
                    {
                        if (randNeighbor_nnarray[j][randNeighbor_nnarray[0].Length - 1] == 1)
                            sln++;
                    }

                    if (sln != 0)
                        sl_ratio = ((double)slp / (double)sln);
                    else
                        sl_ratio = Double.PositiveInfinity;

                    if (sl_ratio == Double.PositiveInfinity && slp == 0)
                        continue;
                    else
                    {
                        for (int atr = 0; atr < trainingSamples[0].Length; atr++)
                        {
                            if (sl_ratio == Double.PositiveInfinity && slp != 0)
                                gap = 0;
                            else if (sl_ratio == 1)
                                gap = random.NextDouble();
                            else if (sl_ratio > 1)
                                gap = random.NextDouble() * (1 / sl_ratio);
                            else if (sl_ratio < 1)
                                gap = random.NextDouble() * (1 - (1 / sl_ratio)) + (1 / sl_ratio);

                            difference = nnarray[rand][atr] - minoritySamples[i][atr];
                            synthetic[newIndex][atr] = Math.Round(minoritySamples[i][atr] + gap * difference, 6);
                        }
                        newIndex++;
                        if (newIndex == synthetic.Length)
                            break;
                    }
                }
            }
            if (newIndex != synthetic.Length)
                goto loop;
            return synthetic;
        }
    }
}
