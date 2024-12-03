//*************************************************************************************
// File:        Randomizer.cs
//*************************************************************************************
// Description: Encapsulates a static access to genuine random number generators.
//*************************************************************************************
// Classes:     Randomizer
//*************************************************************************************
// Use:         Randomizer represents a genuine random number generator.
//              Instancig using static class.
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************

using ARWNI2S.Infrastructure.Interop;

namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Encapsulates a static access to genuine random number generators.
    /// </summary>
    public class Randomizer
    {
        #region Static class members

        private static Random _seedGenerator = null;
        private static Randomizer _global = null;

        public static Randomizer Global
        {
            get
            {
                if (_global == null)
                    _global = new Randomizer(_seedGenerator.Next());
                return _global;
            }
        }
        static Randomizer()
        {
            int seed = 1234567890;

            if (NativeMethods.QueryPerformanceCounter(out long qpf))
            {
                seed = (int)(qpf - (qpf >> 32 << 32));
            }

            _seedGenerator = new Random(seed);
        }

        /// <summary>
        /// Devuelve una instancia de la clase <see cref="Randomizer"/> inicializada
        /// con un numero aleatoreo.
        /// </summary>
        /// <returns>Una instancia de la clase <see cref="Randomizer"/></returns>
        public static Randomizer CreateRandomizer()
        {
            return new Randomizer(_seedGenerator.Next());
        }

        #endregion

        private Random _rnd;

        // Not Creatable.
        private Randomizer(int seed)
        {
            _rnd = new Random(seed);
        }

        /// <summary>
        /// Generates a boolean value considering a 0.5 (50%) success rate.
        /// </summary>
        public bool GetBooleean()
        {
            return GetBooleean(0.5);
        }

        /// <summary>
        /// Generates a boolean value considering the success rate.
        /// </summary>
        /// <param name="successRate"></param>
        /// <returns></returns>
        public bool GetBooleean(double successRate)
        {
            bool value = true;
            if (_rnd.NextDouble() < successRate)
                value = false;
            return value;
        }

        /// <summary>
        /// Returns a float value between 0.0 y 1.0
        /// </summary>
        /// <returns>Value between 0.0 y 1.0</returns>
        public float GetFloat()
        {
            return (float)_rnd.NextDouble();
        }
        /// <summary>
        /// Returns a double value between 0.0 y 1.0
        /// </summary>
        /// <returns>Value between 0.0 y 1.0</returns>
        public double GetDouble()
        {
            return _rnd.NextDouble();
        }
        /// <summary>
        /// Generate random double numbers between min and max
        /// </summary>
        /// <param name="minimum">Minimum value</param>
        /// <param name="maximum">Maximum value</param>
        /// <returns>Double</returns>
        public double GetRandomDoubleNumber(double minimum, double maximum)
        {
            return _rnd.NextDouble() * (maximum - minimum) + minimum;
        }
        /// <summary>
        /// Returns a integer value.
        /// </summary>
        /// <returns>Integer value</returns>
        public int GetInt()
        {
            return _rnd.Next();
        }
        /// <summary>
        /// Returns a integer value between 0 and <paramref name="maxValue"/> parameter.
        /// </summary>
        /// <param name="maxValue">A integer value that represents the max value for the random number.</param>
        /// <returns>Value between 0 and <paramref name="maxValue"/></returns>
        public int GetInt(int maxValue)
        {
            return _rnd.Next(maxValue);
        }
        /// <summary>
        /// Returns a byte matrix with size set by <paramref name="numBytes"/> parameter.
        /// </summary>
        /// <param name="numBytes">Byte matrix size.</param>
        /// <returns>A byte matrix with size set by <paramref name="numBytes"/> parameter, filled with random values.</returns>
        public byte[] GetBytes(int numBytes)
        {
            byte[] buff = new byte[numBytes];
            _rnd.NextBytes(buff);
            return buff;
        }
    }
}