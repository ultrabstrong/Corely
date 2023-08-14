namespace Corely.Shared.Models.Responses
{
    public class PagedResponse<T> where T : class
    {
        private int _skip;
        private readonly int _take;

        public List<T> Items { get; private set; } = new List<T>();
        public int PageNum => _skip / _take;
        public bool HasMore { get; private set; } = true;

        public delegate PagedResponse<T> GetNextChunkDelegate(PagedResponse<T> currentChunk);
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

        public PagedResponse(int skip, int take)
        {
            _skip = skip;
            _take = take;
        }

        public void SetItems(List<T> items)
        {
            UpdatePage(items?.Count ?? 0);
            if (items != null)
            {
                Items = items;
            }
        }

        public void AddItems(List<T> items)
        {
            UpdatePage(items?.Count ?? 0);
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

        public PagedResponse<T>? GetNextChunk()
        {
            if (HasMore == false || _onGetNextChunk == null)
            {
                return null;
            }
            return _onGetNextChunk?.Invoke(this);
        }
    }
}
