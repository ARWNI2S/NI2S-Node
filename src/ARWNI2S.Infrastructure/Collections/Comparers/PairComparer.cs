namespace ARWNI2S.Collections.Comparers
{
    /// <summary>
    /// Class to change an IComparer&lt;TKey&gt; and IComparer&lt;TValue&gt; to an IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt; 
    /// Keys are compared, followed by values.
    /// </summary>
    [Serializable]
    internal class PairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IComparer<TKey> _keyComparer;
        private readonly IComparer<TValue> _valueComparer;

        public PairComparer(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
        {
            _keyComparer = keyComparer;
            _valueComparer = valueComparer;
        }

        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            int keyCompare = _keyComparer.Compare(x.Key, y.Key);

            if (keyCompare == 0)
                return _valueComparer.Compare(x.Value, y.Value);
            else
                return keyCompare;
        }

        public override bool Equals(object obj)
        {
            if (obj is PairComparer<TKey, TValue>)
                return Equals(_keyComparer, ((PairComparer<TKey, TValue>)obj)._keyComparer) &&
                    Equals(_valueComparer, ((PairComparer<TKey, TValue>)obj)._valueComparer);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _keyComparer.GetHashCode() ^ _valueComparer.GetHashCode();
        }
    }

}
