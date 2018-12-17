
namespace Framework
{
    public class HexaGrid<T>
    {
        private readonly T[,] _innerGrid;

        public HexaGrid(int q, int r)
        {
            _innerGrid = new T[q, r];
        }

        public T this[int q, int r]
        {
            get
            {
                if (q < 0 || r < 0 || q >= _innerGrid.GetLength(0) || r >= _innerGrid.GetLength(1))
                    return default;
                return _innerGrid[q, r];
            }
            set => _innerGrid[q, r] = value;
        }

        public int GetLength(int dimension)
        {
            return _innerGrid.GetLength(dimension);
        }
    }
}
