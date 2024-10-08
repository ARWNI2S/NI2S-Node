using ARWNI2S.Infrastructure.Collections.Internals;

namespace ARWNI2S.Infrastructure.Collections.Comparers
{
    /// <summary>
    /// Class to change an IEqualityComparer&lt;TKey&gt; to an IEqualityComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt; 
    /// Only the keys are compared.
    /// </summary>
    [Serializable]
    internal class KeyValueEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IEqualityComparer<TKey> _keyEqualityComparer;

        public KeyValueEqualityComparer(IEqualityComparer<TKey> keyEqualityComparer)
        { _keyEqualityComparer = keyEqualityComparer; }

        public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        { return _keyEqualityComparer.Equals(x.Key, y.Key); }

        public int GetHashCode(KeyValuePair<TKey, TValue> obj)
        {
            return CollectionUtils.GetHashCode(obj.Key, _keyEqualityComparer);
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyValueEqualityComparer<TKey, TValue>)
                return Equals(_keyEqualityComparer, ((KeyValueEqualityComparer<TKey, TValue>)obj)._keyEqualityComparer);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _keyEqualityComparer.GetHashCode();
        }
    }
}
