using System.Threading;

namespace Navigacny_system.Simulation
{
    /// <summary>
    /// Create class which is able to stop the tread
    /// </summary>
    /// <autor> Lukáš Kabát </autor>
    /// <copyright> GNU General Public License v3.0 </copyright>
    public class CancelToken
    {
        public CancellationTokenSource Source { get; set; }

        /// <summary>
        /// Contains token which is able to stop thread
        /// </summary>
        /// <param name="source"></param>
        public CancelToken(CancellationTokenSource source)
        {
            Source = source;
        }
    }
}