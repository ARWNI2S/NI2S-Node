namespace NI2S.Node
{
    internal static class InternerConstants
    {
        /* Recommended cache sizes, based on expansion policy of ConcurrentDictionary
        // Internal implementation of ConcurrentDictionary resizes to prime numbers (not divisible by 3 or 5 or 7)
        31
        67
        137
        277
        557
        1,117
        2,237
        4,477
        8,957
        17,917
        35,837
        71,677
        143,357
        286,717
        573,437
        1,146,877
        2,293,757
        4,587,517
        9,175,037
        18,350,077
        36,700,157
        */
        private const int SIZE_TINY = 31;
        public const int SIZE_SMALL = 67;
        private const int SIZE_SMALL1 = 137;
        private const int SIZE_SMALL2 = 277;
        private const int SIZE_SMALL3 = 557;
        public const int SIZE_MEDIUM = 1117;
        private const int SIZE_MEDIUM1 = 2237;
        private const int SIZE_MEDIUM2 = 4477;
        private const int SIZE_MEDIUM3 = 8957;
        private const int SIZE_MEDIUM4 = 17917;
        private const int SIZE_MEDIUM5 = 35837;
        private const int SIZE_MEDIUM6 = 71677;
        public const int SIZE_LARGE = 143357;
        private const int SIZE_LARGE1 = 286717;
        private const int SIZE_LARGE2 = 573437;
        private const int SIZE_LARGE3 = 1146877;
        public const int SIZE_X_LARGE = 2293757;
        private const int SIZE_X_LARGE1 = 4587517;
        private const int SIZE_X_LARGE2 = 9175037;
        private const int SIZE_X_LARGE3 = 18350077;
        private const int SIZE_X_LARGE4 = 36700157;

        public static readonly TimeSpan DefaultCacheCleanupFreq = TimeSpan.FromMinutes(10);
    }
}
