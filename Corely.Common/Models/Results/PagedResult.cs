namespace Corely.Common.Models.Results
{
    public class PagedResult<T>
    {
        private int _skip;
        private readonly int _take;

        public List<T> Items { get; private set; } = new List<T>();
        public int Skip => _skip;
        public int Take => _take;
        public int PageNum => (int)Math.Ceiling((decimal)_skip / _take);
        public bool HasMore { get; private set; } = true;

        public delegate PagedResult<T> GetNextChunkDelegate(PagedResult<T> currentChunk);
        public event GetNextChunkDelegate OnGetNextChunk
        {
            add
            {
                _onGetNextChunk -= value;
                _onGetNextChunk += value;
            }
            remove
            {
                _onGetNextChunk -= value;
            }
        }
        private event GetNextChunkDelegate _onGetNextChunk;

        public PagedResult(int skip, int take)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(skip, nameof(skip));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(take, nameof(take));

            _skip = skip;
            _take = take;
        }

        public PagedResult<T>? GetNextChunk()
        {
            if (_onGetNextChunk == null)
            {
                throw new InvalidOperationException($"{nameof(OnGetNextChunk)} must be set first");
            }
            if (HasMore == false)
            {
                return null;
            }
            return _onGetNextChunk?.Invoke(this);
        }

        public void SetItems(IEnumerable<T> items)
        {
            UpdatePage(items?.Count() ?? 0);
            if (items != null)
            {
                Items = new(items);
            }
        }

        public void AddItems(IEnumerable<T> items)
        {
            UpdatePage(items?.Count() ?? 0);
            if (items != null)
            {
                Items.AddRange(items);
            }
        }

        private void UpdatePage(int itemscount)
        {
            _skip += itemscount;
            HasMore = itemscount == _take;
        }
    }
}
