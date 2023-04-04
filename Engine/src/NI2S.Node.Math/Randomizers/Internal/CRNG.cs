// TODO: find and replace this line with COPYRIGTH NOTICE entire solution

// This code was contributed by sapnasingh4991

namespace NI2S.Mathematics.Randomizers.Internal
{
    // Congruential Random Number Generator.
    internal sealed class CRNG
    {
        private readonly int _seed, _mod, _mult, _incr;
        private int last;

        // NOT CREATABLE
        private CRNG(int seed) : this(seed, seed, seed, seed) { }
        private CRNG(int seed, int mod, int mult, int incr)
        {
            _seed = seed;
            _mod = mod;
            _mult = mult;
            _incr = incr;
            last = _seed;
        }

        internal int Next()
        {
            // Follow the linear congruential method
            last = (last * _mult + _incr) % _mod;
            return last;
        }

        private static CRNG _default;
        public static CRNG Default
        {
            get
            {
                if (_default == null)
                    _default = new CRNG(Randomizer.GetRandomSeed());
                return _default;
            }
        }

        /// <summary>
        /// Creates a new Congruential Random Number Generator (<see cref="CRNG"/>) with a time random seed.
        /// </summary>
        /// <returns>A <see cref="CRNG"/> instance with a time random seed and identical generation values.</returns>
        public static CRNG Create() => Create(Randomizer.GetRandomSeed());

        /// <summary>
        /// Creates a new Congruential Random Number Generator (<see cref="CRNG"/>) with a provided <paramref name="seed"/> value.
        /// </summary>
        /// <param name="seed">The provided seed value, used for all inner generation values.</param>
        /// <returns>A <see cref="CRNG"/> instance with a provided <paramref name="seed"/> and identical generation values.</returns>
        public static CRNG Create(int seed) { return new CRNG(seed); }

        /// <summary>
        /// Creates a new Congruential Random Number Generator (<see cref="CRNG"/>) with customized generation values.
        /// </summary>
        /// <remarks>Use this method with <b>exact</b> coincidence for a <b>fully predictable</b> generator.</remarks>
        /// <param name="seed">The provided seed value.</param>
        /// <param name="mod">The provided modulus term value.</param>
        /// <param name="mult">The provided multiplier term value.</param>
        /// <param name="incr">The provided increment term value.</param>
        /// <returns>A <see cref="CRNG"/> instance with a provided set of generation terms.</returns>
        public static CRNG Create(int seed, int mod, int mult, int incr) { return new CRNG(seed, mod, mult, incr); }
    }
}