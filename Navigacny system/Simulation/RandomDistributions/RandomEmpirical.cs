using System;
using System.Collections.Generic;

namespace Navigacny_system.Simulation.RandomDistributions
{
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class RandomEmpirical:IRandomDistribution
    {
        private readonly List<Tuple<IRandomDistribution, double>> distributionProbList;
        private readonly RandomUniformContinuous rng;

        /// <summary>
        /// Create new empirical random generator,which will generate random value
        /// </summary>
        /// <param name="distributionProbList">List of pairs of random generators and positive
        /// probability of selecting this generator for generating values which sum need to be 1</param>
        public RandomEmpirical(List<Tuple<IRandomDistribution, double>> distributionProbList)
        {
            this.distributionProbList = distributionProbList;
            rng = new RandomUniformContinuous(null,null);
        }
        public double GetValue()
        {
            //<0,1)
            double genValue = rng.GetValue();
            double probSum = 0;
            for (int i = 0; i < distributionProbList.Count; i++)
            {
                if (genValue < probSum + distributionProbList[i].Item2)
                {
                    return distributionProbList[i].Item1.GetValue();
                }
                else
                {
                    probSum += distributionProbList[i].Item2;
                }
            }
            throw new Exception("Empirical distribution: Probability sum is less than 1");
        }
    }
}