using JetBrains.Annotations;

namespace X.Table {
    [PublicAPI]
    public sealed class Paging {
        private readonly int _maxSize;
        private          int _size;

        public Paging(int index = 0, int size = 25, int maxSize = 25) {
            Index    = index;
            Size     = size;
            _maxSize = maxSize;
        }

        public int Index { get; set; }

        public int Size {
            get => _size;
            set => _size = value < _maxSize ? value : _maxSize;
        }
    }
}
