using System;

namespace Navigacny_system.Simulation.RandomDistributions
{
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class RandomUniformDiscrete : IRandomDistribution
    {
        private readonly Random rng;
        private readonly int? min;
        private readonly int? max;

        /// <summary>
        /// Create new random discrete generator which generate values from min (included) to max (included)
        /// </summary>
        /// <param name="min">Generate values from min (included) or int.MinValue if null</param>
        /// <param name="max">Generate values from max (included) or int.MaxValue if null</param>
        public RandomUniformDiscrete(int? min, int? max)
        {
            rng = new Random(SeedGenerator.GetNextSeed());
            this.min = min;
            this.max = max + 1;
        }

        /// <summary>
        /// Generate value from generator,which can be safely type-cast to int
        /// </summary>
        /// <returns>Value from generator,which can be safely type-cast to int</returns>
        public double GetValue()
        {
            if (max != null)
            {
                if (min != null)
                    return rng.Next(min.Value, max.Value);
                else
                    return rng.Next(max.Value);
            }
            else
            {
                if (min != null)
                    return rng.Next(min.Value, int.MaxValue);
                else
                    return rng.Next();
            }
        }
    }
}