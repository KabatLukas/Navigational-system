using System.Collections.Generic;

namespace Navigacny_system.Simulation
{
    /// <summary>
    /// Abstract class which serves as baseline for all simulations
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public abstract class SimulationCore
    {
        /// <summary>
        /// Contain list of average result of the simulation
        /// </summary>
        public List<double> Average { get; }

        /// <summary>
        /// Event which fires every time average value changes
        /// </summary>
        public event AverageDelegate AverageChangedE;
        public delegate void AverageDelegate(List<double> newAverages,int replicationN);

        /// <summary>
        /// Abstract class which serves as baseline for all simulations
        /// </summary>
        protected SimulationCore()
        {
            Average = new List<double>();
        }

        /// <summary>
        /// Run the simulation
        /// </summary>
        /// <param name="numOfReplications">How many replication will run</param>
        /// <param name="cancellationToken">Token to enable stopping of simulation</param>
        public void Simulate(int numOfReplications, CancelToken cancellationToken)
        {
            BeforeSimulation();
            Average.Clear();
            List<double> resultSum = new List<double>();
            for (int replicationIndex = 0; replicationIndex < numOfReplications; replicationIndex++)
            {
                List<double> replicationResult = Replication();

                for (int j = 0; j < replicationResult.Count; j++)
                {
                    //only if its first result set
                    if (j == resultSum.Count)
                    {
                        resultSum.Add(replicationResult[j]);
                        Average.Add(replicationResult[j]);
                    }
                    else
                    {
                        resultSum[j] += replicationResult[j];
                        Average[j] = resultSum[j] / (replicationIndex + 1);
                    }
                }
                AverageChangedE?.Invoke(Average, replicationIndex + 1);
                if (cancellationToken.Source.Token.IsCancellationRequested)
                    break;
            }
            AfterSimulation();
        }

        public abstract List<double> Replication();

        /// <summary>
        /// Empty method which execute just at the start of the simulation
        /// </summary>
        public virtual void BeforeSimulation()
        {
        }

        /// <summary>
        /// Empty method which execute just at the end of simulation
        /// </summary>
        public virtual void AfterSimulation()
        {
        }
    }
}