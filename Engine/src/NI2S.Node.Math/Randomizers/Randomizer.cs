// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Mathematics.Randomizers
{
    internal static class Randomizer
    {
        private static Random _timeRandom = new(DateTime.UtcNow.Microsecond);

        internal static int GetRandomSeed()
        {
            return _timeRandom.Next(int.MaxValue);
        }
    }
}
