namespace ARWNI2S.Infrastructure.Collections.Comparers
{
    /// <summary>
    /// Class to change an Comparison&lt;TKey&gt; to an IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;.
    /// GetHashCode cannot be used on this class.
    /// </summary>
    [Serializable]
    internal class ComparisonKeyValueComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly Comparison<TKey> _comparison;

        public ComparisonKeyValueComparer(Comparison<TKey> comparison)
        { _comparison = comparison; }

        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        { return _comparison(x.Key, y.Key); }

        public override bool Equals(object obj)
        {
            if (obj is ComparisonKeyValueComparer<TKey, TValue>)
                return _comparison.Equals(((ComparisonKeyValueComparer<TKey, TValue>)obj)._comparison);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _comparison.GetHashCode();
        }
    }
}
