using System;

namespace Navigacny_system.Simulation.RandomDistributions
{
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public interface IRandomDistribution
    {
        /// <summary>
        /// Get random number from random number generator
        /// </summary>
        /// <returns></returns>
        double GetValue();
    }
}