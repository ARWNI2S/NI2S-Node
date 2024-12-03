namespace ARWNI2S.Collections.Comparers
{
    /// <summary>
    /// Class to change an Comparison&lt;T&gt; to an IComparer&lt;T&gt;.
    /// </summary>
    [Serializable]
    internal class ComparisonComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        { _comparison = comparison; }

        public int Compare(T x, T y)
        { return _comparison(x, y); }

        public override bool Equals(object obj)
        {
            if (obj is ComparisonComparer<T>)
                return _comparison.Equals(((ComparisonComparer<T>)obj)._comparison);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _comparison.GetHashCode();
        }
    }
}
