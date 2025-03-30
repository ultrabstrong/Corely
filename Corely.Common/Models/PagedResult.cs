namespace Corely.Common.Models;

public class PagedResult<T>
{
    private int _skip;
    private readonly int _take;

    public List<T> Items { get; private set; } = [];
    public int Skip => _skip;
    public int Take => _take;
    public int PageNum => (int)Math.Ceiling((decimal)_skip / _take);
    public bool HasMore { get; private set; } = true;

    public delegate PagedResult<T> GetNextPageDelegate(PagedResult<T> currentPage);
    public event GetNextPageDelegate OnGetNextPage
    {
        add
        {
            _onGetNextPage -= value; // Ensure only one subscriber
            _onGetNextPage += value;
        }
        remove
        {
            _onGetNextPage -= value;
        }
    }
    private event GetNextPageDelegate _onGetNextPage = null!;

    public PagedResult(int skip, int take)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(skip, nameof(skip));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(take, nameof(take));

        _skip = skip;
        _take = take;
    }

    public PagedResult<T>? GetNextPage()
    {
        if (_onGetNextPage == null)
        {
            throw new InvalidOperationException($"{nameof(OnGetNextPage)} must be set first");
        }
        if (HasMore == false)
        {
            return null;
        }
        return _onGetNextPage?.Invoke(this);
    }

    public void SetItems(IEnumerable<T> items)
    {
        UpdatePage(items?.Count() ?? 0);
        if (items != null)
        {
            Items = [.. items];
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
