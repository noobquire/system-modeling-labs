using static System.Math;

namespace SystemModelingLabs.Utils
{
    public class RandomUtils
    {
        /// <summary>
        /// Generates random numbers with exponential distribution.
        /// </summary>
        /// <param name="lambda">Lambda parameter of exp distribution</param>
        /// <returns>Random number with exponential distribution.</returns>
        public static double NextExponential(double lambda)
        {
            var rng = new Random();
            return -Log(rng.NextDouble()) / lambda;
        }

        /// <summary>
        /// Generates random numbers with normal distribution.
        /// </summary>
        /// <param name="mu">Mean value of distribution.</param>
        /// <param name="sigma">Standard deviation of distribution.</param>
        /// <returns>Random number with normal distribution.</returns>
        public static double NextGaussian(double mu, double sigma)
        {
            var rng = new Random();
            var randomValues = Enumerable.Range(0, 12)
                .Select(i => rng.NextDouble());
            return sigma * (randomValues.Sum() - 6) + mu;
        }
    }
}
