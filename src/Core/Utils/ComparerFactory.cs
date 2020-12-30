using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace X.Core.Utils {
    public static class ComparerFactory<T> {
        /// <summary>
        /// Create a key based equability comparer implementation.
        /// </summary>
        /// <param name="keyGetter"></param>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <returns>IEqualityComparer implementation.</returns>
        public static IEqualityComparer<T> Create<TKey>(Func<T, TKey> keyGetter)
            => new KeyBasedEqualityComparer<T, TKey>(keyGetter);

        /// <summary>
        /// Create an equability comparer implementation using comparision function
        /// and hash code generator function.
        /// </summary>
        /// <param name="comparisonFunc"></param>
        /// <param name="getHashCode"></param>
        /// <returns></returns>
        public static IEqualityComparer<T> Create(
            Func<T, T, bool> comparisonFunc,
            Func<T, int> getHashCode
        )
            => new ComparisonFuncComparer<T>(comparisonFunc, getHashCode);
    }

    internal class ComparisonFuncComparer<T> : IEqualityComparer<T> {
        private readonly Func<T, T, bool> _comparisonFunc;
        private readonly Func<T, int>     _hashFunc;

        public ComparisonFuncComparer(Func<T, T, bool> func, Func<T, int> hashFunc) {
            _comparisonFunc = func;
            _hashFunc       = hashFunc;
        }

        public bool Equals(T? x, T? y) {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return _comparisonFunc(x, y);
        }

        public int GetHashCode(T obj) {
            Guard.Against.Null(obj, nameof(obj));
            return _hashFunc(obj).GetHashCode();
        }
    }

    internal class KeyBasedEqualityComparer<T, TKey> : IEqualityComparer<T> {
        private readonly Func<T, TKey> _keyGetter;

        public KeyBasedEqualityComparer(Func<T, TKey> keyGetter) => _keyGetter = keyGetter;

        public bool Equals(T? x, T? y) {
            if (x is null && y is null)
                return true;

            if (x is null || y is null)
                return false;

            return EqualityComparer<TKey>.Default.Equals(_keyGetter(x), _keyGetter(y));
        }

        public int GetHashCode(T obj) {
            var key = _keyGetter(obj);

            return key is null ? 0 : key.GetHashCode();
        }
    }
}