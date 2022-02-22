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

        /// <summary>
        /// Generates random numbers with uniform distribution.
        /// </summary>
        /// <param name="min">Minimum generated value.</param>
        /// <param name="max">Maximum generated value.</param>
        /// <returns>Random number with uniform distribution.</returns>
        public static double NextUniform(double min, double max)
        {
            var rng = new Random();
            var randomValue = rng.NextDouble();
            return randomValue * max + min;
        }


        /// <summary>
        /// Generates random numbers with Erlang distribution.
        /// </summary>
        /// <param name="k">Amount of iterations.</param>
        /// <param name="mu">Mean value.</param>
        /// <returns>Random number with uniform distribution.</returns>
        public static double NextErlang(double mu, int k)
        {
            var rng = new Random();
            var randomValue = rng.NextDouble();
            var sum = Enumerable.Range(0, k)
                .Select(i => rng.NextDouble())
                .Sum();
            return -sum / (k * mu);
        }
    }
}
