namespace ARWNI2S.Infrastructure.Collections.Comparers
{
    /// <summary>
    /// Class to change an IComparer&lt;TKey&gt; to an IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt; 
    /// Only the keys are compared.
    /// </summary>
    [Serializable]
    internal class KeyValueComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IComparer<TKey> _keyComparer;

        public KeyValueComparer(IComparer<TKey> keyComparer)
        { _keyComparer = keyComparer; }

        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        { return _keyComparer.Compare(x.Key, y.Key); }

        public override bool Equals(object obj)
        {
            if (obj is KeyValueComparer<TKey, TValue>)
                return Equals(_keyComparer, ((KeyValueComparer<TKey, TValue>)obj)._keyComparer);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _keyComparer.GetHashCode();
        }
    }
}
