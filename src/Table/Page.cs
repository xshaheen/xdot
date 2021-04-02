using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace X.Table {
    [PublicAPI]
    public class Page<T> {
        public Page(List<T> items, int index, int size, int totalItems) {
            Items      = items;
            Index      = index;
            Size       = size;
            TotalItems = totalItems;
        }

        public List<T> Items { get; }

        public int Index { get; }

        public int Size { get; }

        public int TotalItems { get; }

        public int TotalPages => (int) Math.Ceiling(TotalItems / (decimal) Size);
    }
}
