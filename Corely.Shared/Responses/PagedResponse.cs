namespace Corely.Shared.Responses
{
    public class PagedResponse<T> where T : class
    {
        public PagedResponse(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public List<T> Items { get; private set; } = new List<T>();
        public int PageNum => Skip / Take;
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool HasMore { get; private set; } = true;

        private void UpdatePage(int itemscount)
        {
            Skip += itemscount;
            HasMore = itemscount == Take;
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

        public PagedResponse<T>? GetNextChunk()
        {
            if (HasMore == false || _onGetNextChunk == null)
            {
                return null;
            }
            return _onGetNextChunk?.Invoke(this);
        }

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

    }
}
