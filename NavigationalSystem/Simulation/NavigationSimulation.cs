using System;
using System.Collections.Generic;
using Navigacny_system.Simulation.RandomDistributions;

namespace Navigacny_system.Simulation
{
    /// <summary>
    /// Class containg the simulation of simple navigational system
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class NavigationSimulation : SimulationCore
    {
        /// <summary>
        /// Maximum amount of time which can truck spend traveling
        /// </summary>
        private static readonly double DURATIONLIMIT = 450;

        /// <summary>
        /// Represent array of unconnected nodes
        /// </summary>
        private readonly List<NavigationNode> map;

        /// <summary>
        /// Translate name of the nodes into integers for better performance
        /// </summary>
        private readonly Dictionary<string, int> nameToIndex;

        /// <summary>
        /// Array of posible routes
        /// </summary>
        private List<NavigationRoute> routes;

        private List<int> durationInLimitSum;
        private List<double> durationInLimitPercent;

        /// <summary>
        /// How many times sumulation has been run
        /// </summary>
        private int ReplicationCount;

        /// <summary>
        /// Event which fires every time limit is hit changes
        /// </summary>
        public event InTimeDelegate InTimeChangedE;
        public delegate void InTimeDelegate(List<double> newAverages);

        /// <summary>
        /// Create new simulation of navigational system based on general simulation core
        /// </summary>
        public NavigationSimulation():base()
        {
            nameToIndex = new Dictionary<string, int>();
            map = new List<NavigationNode>()
            {
                new NavigationNode("A"), //0
                new NavigationNode("B"), //1
                new NavigationNode("C"), //2
                new NavigationNode("D"), //3
                new NavigationNode("E"), //4
                new NavigationNode("F"), //5
                new NavigationNode("G"), //6
                new NavigationNode("H")  //7
            };

            for (int i = 0; i < map.Count; i++)
            {
                nameToIndex.Add(map[i].Name, i);
            }
        }

        /// <summary>
        /// Creates all routes
        /// </summary>
        private void CreateRoutes()
        {
            routes = new List<NavigationRoute>
            {
                new NavigationRoute(new Tuple<List<string>,double>(new List<string> {"A", "B", "C", "D", "E"},1)),
                new NavigationRoute(new Tuple<List<string>,double>(new List<string> {"A", "F", "G", "E"},1)),
                new NavigationRoute(new Tuple<List<string>,double>(new List<string>  {"A", "F", "H", "D", "E"},0.95),
                    new Tuple<List<string>,double>(new List<string>  { "A", "F", "H", "C","D", "E"},0.05)),
                new NavigationRoute(new Tuple<List<string>,double>(new List<string> {"A", "F", "H", "C","D", "E"},1))
            };
        }

        /// <summary>
        /// Create all links between nodes
        /// </summary>
        private void CreateConnections()
        {
            CreateConnection("A", "B", new RandomUniformContinuous(170, 217)); //A-B
            CreateConnection("B", "C", new RandomUniformContinuous(120, 230)); //B-C
            CreateConnection("C", "D", new RandomUniformContinuous(50, 70)); //C-D
            CreateConnection("D", "E", new RandomUniformContinuous(19, 36)); //D-E
            CreateConnection("A", "F", new RandomUniformContinuous(150, 240)); //A-F   
            CreateConnection("F", "G",
                new RandomEmpirical(new List<Tuple<IRandomDistribution, double>>
                {
                    new Tuple<IRandomDistribution, double>(
                        new RandomUniformDiscrete(170, 195), 0.2),
                    new Tuple<IRandomDistribution, double>(
                        new RandomUniformDiscrete(196, 280), 0.8)
                })); //F-G
            CreateConnection("F", "H", new RandomUniformContinuous(30, 62)); //F-H
            CreateConnection("H", "C", new RandomUniformContinuous(150, 220)); //H-C
            CreateConnection("H", "D", new RandomUniformContinuous(170, 200)); //H-D
            CreateConnection("G", "E", new RandomUniformDiscrete(20, 49)); //G-E
        }

        private void CreateConnection(string fromName, string toName, IRandomDistribution distribution)
        {
            map[nameToIndex[fromName]].AddConnection(map[nameToIndex[toName]], distribution);
        }

        /// <summary>
        /// Reset all generators before simulation
        /// </summary>
        public override void BeforeSimulation()
        {
            CreateRoutes();
            CreateConnections();
            durationInLimitSum = new List<int>(routes.Count);
            durationInLimitPercent = new List<double>(routes.Count);
            ReplicationCount = 0;
        }


        /// <summary>
        /// Simulate one replication of simulation
        /// </summary>
        /// <returns>List of times which each route taken</returns>
        public override List<double> Replication()
        {
            //Generate map times
            for (int i = 0; i < map.Count; i++)
            {
                foreach (var connection in map[i].ConnectToList)
                {
                    connection.GenerateTime();
                }
            }
            ReplicationCount++;
            List<double> durationList = new List<double>();
            //for each route
            for (int routeI = 0; routeI < routes.Count; routeI++)
            {
                double duration = 0;
                List<string> route = routes[routeI].GetRoute();
                //for each node in route
                for (int nodeFromI = 0; nodeFromI < route.Count - 1; nodeFromI++)
                {
                    
                    string nameFrom = route[nodeFromI];
                    string nameTo = route[nodeFromI+1];
                    duration += map[nameToIndex[nameFrom]].ConnectToList.Find(e => e.To.Name == nameTo).Time;
                }
                durationList.Add(duration);
                if (durationInLimitSum.Count < routes.Count)
                {
                    durationInLimitSum.Add(duration < DURATIONLIMIT ? 1 : 0);
                    durationInLimitPercent.Add((double)durationInLimitSum[routeI]/ReplicationCount);
                }
                else
                {
                    durationInLimitSum[routeI] += (duration < DURATIONLIMIT) ? 1 : 0;
                    durationInLimitPercent[routeI] = (double)durationInLimitSum[routeI] / ReplicationCount;
                }
            }
            InTimeChangedE?.Invoke(durationInLimitPercent);
            return durationList;
        }
    }
}