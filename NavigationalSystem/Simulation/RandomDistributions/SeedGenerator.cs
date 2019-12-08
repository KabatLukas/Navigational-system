using System;

namespace Navigacny_system.Simulation.RandomDistributions
{
    /// <summary>
    /// Static seed generator for random number generator
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public static class SeedGenerator
    {
        private static readonly Random SeedGen = new Random();

        /// <summary>
        /// Get next random value for seed in new random number generator
        /// </summary>
        /// <returns></returns>
        public static int GetNextSeed()
        {
            return SeedGen.Next();
        }
    }
}