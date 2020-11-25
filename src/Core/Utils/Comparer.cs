using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace X.Core.Utils
{
    public class Comparer
    {
        public static Comparer<T> GetIEqualityComparer<T>(Func<T?, T?, bool> func) => new Comparer<T>(func);
    }

    public class Comparer<T> : Comparer, IEqualityComparer<T>
    {
        private readonly Func<T?, T?, bool> _comparisonFunc;

        public Comparer(Func<T?, T?, bool> func) { _comparisonFunc = func; }

        public bool Equals(T? x, T? y) => _comparisonFunc(x, y);

        public int GetHashCode(T obj)
        {
            Guard.Against.Null(obj, nameof(obj));
            return obj.GetHashCode();
        }
    }
}
