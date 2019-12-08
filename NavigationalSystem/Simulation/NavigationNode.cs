using System.Collections.Generic;
using Navigacny_system.Simulation.RandomDistributions;

namespace Navigacny_system.Simulation
{
    /// <summary>
    /// The class which represent node on the map with one way connections to other nodes
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class NavigationNode
    {
        public string Name { get; set; }
        public List<Connection> ConnectToList { get; set; }
        
        public NavigationNode(string name)
        {
            Name = name;
            ConnectToList = new List<Connection>();
        }

        public void AddConnection(NavigationNode newNode, IRandomDistribution timeDistribution)
        {
            ConnectToList.Add(new Connection(newNode, timeDistribution));
        }
    }
}