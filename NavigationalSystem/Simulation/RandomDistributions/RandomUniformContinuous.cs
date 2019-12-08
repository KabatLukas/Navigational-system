using System;

namespace Navigacny_system.Simulation.RandomDistributions
{
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class RandomUniformContinuous : IRandomDistribution
    {
        private readonly Random rng;
        private readonly double? min;
        private readonly double? max;

        /// <summary>
        /// Create new random continuous generator which generate values from min included to max excluded
        /// </summary>
        /// <param name="min">Generate values from min (included) or 0 (included) if null</param>
        /// <param name="max">Generate values from max (excluded) or 1 (excluded) if null</param>
        public RandomUniformContinuous(double? min, double? max)
        {
            rng = new Random(SeedGenerator.GetNextSeed());
            this.min = min;
            this.max = max;
        }

        public double GetValue()
        {
            if (max != null)
            {
                if (min != null)
                    return rng.NextDouble() * (max.Value - min.Value) + min.Value;
                else
                    return rng.NextDouble() * max.Value;
            }
            else
            {
                if (min != null)
                    return rng.NextDouble() * (1 - min.Value) + min.Value;
                else
                    return rng.NextDouble();
            }
        }
    }
}