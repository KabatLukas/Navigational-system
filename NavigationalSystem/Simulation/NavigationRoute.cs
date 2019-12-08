using System;
using System.Collections.Generic;
using Navigacny_system.Simulation.RandomDistributions;

namespace Navigacny_system.Simulation
{
    public class NavigationRoute
    {
        /// <summary>
        /// Pair of routes node names and probability of use of this routes ie. routes can be chosen randomly based on probability
        /// Probability must be positive and sum of probabilities must be 1
        /// </summary>
        /// <autor> Lukáš Kabát </autor>
        /// <copyright> GNU General Public License v3.0 </copyright>
        private Tuple<List<string>,double>[] routes;

        /// <summary>
        /// 0(inclusive) to 1 (exclusive) random number genrator for determining route which will truck choose
        /// </summary>
        private RandomUniformContinuous rng;

        /// <summary>
        /// Create new routes for navigation
        /// </summary>
        /// <param name="routes">Pair of routes node names and probability of use of this routes ie. routes can be chosen randomly based on probability
        /// Probability must be positive and sum of probabilities must be 1</param>
        public NavigationRoute(params Tuple<List<string>, double>[] routes)
        {
            this.routes = routes;
            rng = new RandomUniformContinuous(null,null);
        }

        /// <summary>
        /// Get one of routes based on probabilities
        /// </summary>
        /// <returns>Selected route</returns>
        public List<string> GetRoute()
        {
            if (routes.Length > 1)
            {
                double randValue = rng.GetValue();
                double probSum = 0;
                for (int i = 0; i < routes.Length; i++)
                {
                    if (randValue < probSum + routes[i].Item2)
                    {
                        return routes[i].Item1;
                    }
                    else
                    {
                        probSum += routes[i].Item2;
                    }
                }

                throw new Exception("Navigational routes: Probability sum is less than 1");
            }
            else if (routes.Length == 1)
                return routes[0].Item1;
            else
                return null;
        }
    }
}