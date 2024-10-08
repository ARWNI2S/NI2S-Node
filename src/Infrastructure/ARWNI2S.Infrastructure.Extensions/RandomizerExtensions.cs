namespace ARWNI2S.Infrastructure.Extensions
{
    /// <summary>
    /// Is added to prevent faulty user entry.
    /// </summary>
    public enum RandomDistribution
    {
        /// <summary>
        /// no distribution
        /// </summary>
        None,
        /// <summary>
        /// Exponential Distribution
        /// </summary>
        Exponential,
        /// <summary>
        /// Normal distribution
        /// </summary>
        Normal,
        /// <summary>
        /// Uniform distribution
        /// </summary>
        Uniform,
        /// <summary>
        /// Gamma distribution
        /// </summary>
        Gamma,
        /// <summary>
        /// ChiSquare distribution
        /// </summary>
        ChiSquare,
        /// <summary>
        /// InverseGamma distribution
        /// </summary>
        InverseGamma,
        /// <summary>
        /// Weibull distribution
        /// </summary>
        Weibull,
        /// <summary>
        /// Cauchy distribution
        /// </summary>
        Cauchy,
        /// <summary>
        /// StudenT distribution
        /// </summary>
        StudentT,
        /// <summary>
        /// Laplace distribution
        /// </summary>
        Laplace,
        /// <summary>
        /// LogNormal distribution
        /// </summary>
        LogNormal,
        /// <summary>
        /// Beta distribution
        /// </summary>
        Beta,
    }

    public static class RandomizerExtensions
    {
        /// <summary> 
        /// In this version overloaded method for ComputeValue is created.
        /// Compute Value takes a List in which the first element is distribution name.
        /// </summary>
        /// <param name="simDist"></param>
        /// <returns></returns>
        public static double ComputeValue(this Randomizer rnd, List<object> simDist)
        {
            double value = 0;
            string distName = (string)simDist[0];
            int numberParams = simDist.Count - 1;
            switch (numberParams)
            {
                case 1:
                    {
                        value = rnd.ComputeValue(distName, (double)simDist[numberParams], 0.0);
                        break;
                    }
                case 2:
                    {
                        value = rnd.ComputeValue(distName, (double)simDist[numberParams - 1], (double)simDist[numberParams]);
                        break;
                    }
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// In this version overloaded method for ComputeValue and more distributions are created.
        /// Generates a random number according with given arguments.
        /// </summary>
        /// <param name="dist"> The name of the distribution which is used to compute the time passed on that arc. </param>
        /// <param name="param1"> First parameter of the distribution. </param>
        /// <param name="param2"> Second parameter of the distribution. (if necessary) </param>
        /// <returns></returns>
        public static double ComputeValue(this Randomizer rnd, string dist, double param1, double param2 = 0.0)
        {
            double value = 0;
            switch (dist.ToLowerInvariant())
            {
                case "exponential":
                    {
                        value = rnd.GetExponential(param1);
                        break;
                    }
                case "normal":
                    {
                        value = rnd.GetNormal(param1, param2);
                        break;
                    }
                case "uniform":
                    {
                        value = rnd.GetUniform(param1, param2);
                        break;
                    }
                case "gamma":
                    {
                        value = rnd.GetGamma(param1, param2);
                        break;
                    }
                case "chisquare":
                    {
                        value = rnd.GetChiSquare(param1);
                        break;
                    }
                case "inversegamma":
                    {
                        value = rnd.GetInverseGamma(param1, param2);
                        break;
                    }
                case "weibull":
                    {
                        value = rnd.GetWeibull(param1, param2);
                        break;
                    }
                case "cauchy":
                    {
                        value = rnd.GetCauchy(param1, param2);
                        break;
                    }
                case "studentt":
                    {
                        value = rnd.GetStudentT(param1);
                        break;
                    }
                case "laplace":
                    {
                        value = rnd.GetLaplace(param1, param2);
                        break;
                    }
                case "lognormal":
                    {
                        value = rnd.GetLogNormal(param1, param2);
                        break;
                    }
                case "beta":
                    {
                        value = rnd.GetBeta(param1, param2);
                        break;
                    }
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// In this version overloaded method for ComputeValue and more distributions are created.
        /// Generates a random number according with given arguments.
        /// </summary>
        /// <param name="distribution">The enumeration of the distribution which is used to compute the time passed on that arc.</param>
        /// <param name="param1"> First parameter of the distribution. </param>
        /// <param name="param2"> Second parameter of the distribution. (if necessary) </param>
        /// <returns></returns>
        public static double ComputeValue(this Randomizer rnd, RandomDistribution distribution, double param1, double param2 = 0.0)
        {
            double value = 0;
            switch (distribution)
            {
                case RandomDistribution.Exponential:
                    {
                        value = rnd.GetExponential(param1);
                        break;
                    }
                case RandomDistribution.Normal:
                    {
                        value = rnd.GetNormal(param1, param2);
                        break;
                    }
                case RandomDistribution.Uniform:
                    {
                        value = rnd.GetUniform(param1, param2);
                        break;
                    }
                case RandomDistribution.Gamma:
                    {
                        value = rnd.GetGamma(param1, param2);
                        break;
                    }
                case RandomDistribution.ChiSquare:
                    {
                        value = rnd.GetChiSquare(param1);
                        break;
                    }
                case RandomDistribution.InverseGamma:
                    {
                        value = rnd.GetInverseGamma(param1, param2);
                        break;
                    }
                case RandomDistribution.Weibull:
                    {
                        value = rnd.GetWeibull(param1, param2);
                        break;
                    }
                case RandomDistribution.Cauchy:
                    {
                        value = rnd.GetCauchy(param1, param2);
                        break;
                    }
                case RandomDistribution.StudentT:
                    {
                        value = rnd.GetStudentT(param1);
                        break;
                    }
                case RandomDistribution.Laplace:
                    {
                        value = rnd.GetLaplace(param1, param2);
                        break;
                    }
                case RandomDistribution.LogNormal:
                    {
                        value = rnd.GetLogNormal(param1, param2);
                        break;
                    }
                case RandomDistribution.Beta:
                    {
                        value = rnd.GetBeta(param1, param2);
                        break;
                    }
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// Produce a uniform random sample from the open interval (0, 1).
        /// </summary>
        /// <returns></returns>
        public static double GetUniform(this Randomizer rnd)
        {
            return rnd.GetDouble();
        }

        /// <summary>
        /// Produce a random sample from the open interval (a, b).
        /// </summary>
        /// <param name="intervalA"></param>
        /// <param name="intervalB"></param>
        /// <returns></returns>
        public static double GetUniform(this Randomizer rnd, double intervalA, double intervalB)
        {
            return intervalA + rnd.GetDouble() * (intervalB - intervalA);
        }

        /// <summary>
        /// Get normal (Gaussian) random sample with mean 0 and standard deviation 1
        /// </summary>
        /// <returns></returns>
        public static double GetNormal(this Randomizer rnd)
        {
            // Use Box-Müller algorithm
            double u1 = rnd.GetUniform();
            double u2 = rnd.GetUniform();
            double r = Math.Sqrt(-2.0 * Math.Log(u1));
            double theta = 2.0 * Math.PI * u2;
            return r * Math.Sin(theta);
        }

        /// <summary>
        ///  Get normal (Gaussian) random sample with specified mean and standard deviation
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <returns></returns>
        public static double GetNormal(this Randomizer rnd, double mean, double standardDeviation)
        {
            if (standardDeviation < 0.0)
            {
                string msg = string.Format("Standard Deviation must be positive. Received {Negative}.", standardDeviation);
                throw new ArgumentOutOfRangeException(msg);
            }
            return mean + standardDeviation * rnd.GetNormal();
        }

        /// <summary>
        /// Get exponential random sample with mean 1
        /// </summary>
        /// <returns></returns>
        public static double GetExponential(this Randomizer rnd)
        {
            return Math.Log(rnd.GetDouble());
        }

        /// <summary>
        /// Get exponential random sample with specified mean
        /// </summary>
        /// <param name="mean"></param>
        /// <returns></returns>
        public static double GetExponential(this Randomizer rnd, double mean)
        {
            return -mean * Math.Log(rnd.GetDouble());
        }

        /// <summary>
        /// Get gamma random sample with parameters
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static double GetGamma(this Randomizer rnd, double shape, double scale)
        {
            // Implementation based on "A Simple Method for Generating Gamma Variables"
            // by George Marsaglia and Wai Wan Tsang.  ACM Transactions on Mathematical Software
            // Vol 26, No 3, September 2000, pages 363-372.

            double d, c, x, xsquared, v, u;

            if (shape >= 1.0)
            {
                d = shape - 1.0 / 3.0;
                c = 1.0 / Math.Sqrt(9.0 * d);
                for (; ; )
                {
                    do
                    {
                        x = rnd.GetNormal();
                        v = 1.0 + c * x;
                    }
                    while (v <= 0.0);
                    v = v * v * v;
                    u = rnd.GetUniform();
                    xsquared = x * x;
                    if (u < 1.0 - .0331 * xsquared * xsquared || Math.Log(u) < 0.5 * xsquared + d * (1.0 - v + Math.Log(v)))
                        return scale * d * v;
                }
            }
            else if (shape <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", shape);
                throw new ArgumentOutOfRangeException(msg);
            }
            else
            {
                double g = rnd.GetGamma(shape + 1.0, 1.0);
                double w = rnd.GetUniform();
                return scale * g * Math.Pow(w, 1.0 / shape);
            }
        }

        /// <summary>
        /// Get Chi Square random sample with parameter
        /// </summary>
        /// <param name="degreesOfFreedom"></param>
        /// <returns></returns>
        public static double GetChiSquare(this Randomizer rnd, double degreesOfFreedom)
        {
            // A chi squared distribution with n degrees of freedom
            // is a gamma distribution with shape n/2 and scale 2.
            return rnd.GetGamma(0.5 * degreesOfFreedom, 2.0);
        }

        /// <summary>
        /// Get inverse gamma random sample with parameters
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static double GetInverseGamma(this Randomizer rnd, double shape, double scale)
        {
            return 1.0 / rnd.GetGamma(shape, 1.0 / scale);
        }

        /// <summary>
        /// Get weibull random sample with parameters
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static double GetWeibull(this Randomizer rnd, double shape, double scale)
        {
            if (shape <= 0.0 || scale <= 0.0)
            {
                string msg = string.Format("Shape and scale parameters must be positive. Recieved shape {0} and scale{1}.", shape, scale);
                throw new ArgumentOutOfRangeException(msg);
            }
            return scale * Math.Pow(-Math.Log(rnd.GetUniform()), 1.0 / shape);
        }

        /// <summary>
        /// Get cauchy random sample with parameters
        /// </summary>
        /// <param name="median"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static double GetCauchy(this Randomizer rnd, double median, double scale)
        {
            if (scale <= 0)
            {
                string msg = string.Format("Scale must be positive. Received {0}.", scale);
                throw new ArgumentException(msg);
            }

            double p = rnd.GetUniform();

            // Apply inverse of the Cauchy distribution function to a uniform
            return median + scale * Math.Tan(Math.PI * (p - 0.5));
        }

        /// <summary>
        /// Get student T random sample with parameters
        /// </summary>
        /// <param name="degreesOfFreedom"></param>
        /// <returns></returns>
        public static double GetStudentT(this Randomizer rnd, double degreesOfFreedom)
        {
            if (degreesOfFreedom <= 0)
            {
                string msg = string.Format("Degrees of freedom must be positive. Received {0}.", degreesOfFreedom);
                throw new ArgumentException(msg);
            }

            // See Seminumerical Algorithms by Knuth
            double y1 = rnd.GetNormal();
            double y2 = rnd.GetChiSquare(degreesOfFreedom);
            return y1 / Math.Sqrt(y2 / degreesOfFreedom);
        }

        /// <summary>
        /// Get laplace random sample with parameters
        /// The Laplace distribution is also known as the double exponential distribution.
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static double GetLaplace(this Randomizer rnd, double mean, double scale)
        {
            double u = rnd.GetUniform();
            return u < 0.5 ?
                mean + scale * Math.Log(2.0 * u) :
                mean - scale * Math.Log(2 * (1 - u));
        }

        /// <summary>
        /// Get LogNormal random sample with parameters
        /// </summary>
        /// <param name="mu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double GetLogNormal(this Randomizer rnd, double mu, double sigma)
        {
            return Math.Exp(rnd.GetNormal(mu, sigma));
        }

        /// <summary>
        /// Get beta random sample with parameters
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double GetBeta(this Randomizer rnd, double a, double b)
        {
            if (a <= 0.0 || b <= 0.0)
            {
                string msg = string.Format("Beta parameters must be positive. Received {0} and {1}.", a, b);
                throw new ArgumentOutOfRangeException(msg);
            }
            // There are more efficient methods for generating beta samples.
            // However such methods are a little more efficient and much more complicated.
            // For an explanation of why the following method works, see
            // http://www.johndcook.com/distribution_chart.html#gamma_beta
            double u = rnd.GetGamma(a, 1.0);
            double v = rnd.GetGamma(b, 1.0);
            return u / (u + v);
        }


    }
}
