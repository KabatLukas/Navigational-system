using Navigacny_system.Simulation.RandomDistributions;

namespace Navigacny_system
{
    /// <summary>
    /// Class which represent connection of the creating node to another node
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class Connection
    {
        /// <summary>
        /// Node to which is this conection lead
        /// </summary>
        public NavigationNode To { get; }
        private IRandomDistribution distribution;
        public double Time { get; private set; }

        /// <summary>
        /// Create connection to node with specified distribuion of travel time
        /// </summary>
        /// <param name="to">Node which will be connected to</param>
        /// <param name="distribution">Random distribution of travel time to node</param>
        public Connection(NavigationNode to, IRandomDistribution distribution)
        {
            To = to;
            this.distribution = distribution;
        }

        /// <summary>
        /// Generate the time which it takes to travel to connected node
        /// </summary>
        public void GenerateTime()
        {
            if (distribution != null)
            {
                Time = distribution.GetValue();
            }
        }
    }
}