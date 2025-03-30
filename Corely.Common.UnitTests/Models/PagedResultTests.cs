using Corely.Common.Models;

namespace Corely.Common.UnitTests.Models;

public class PagedResultTests
{
    private PagedResult<object> _pagedResponse;
    private readonly List<object> _testData;

    public PagedResultTests()
    {
        _testData = [];
        for (int i = 0; i < 100; i++)
        {
            _testData.Add($"test{i}");
        }

        _pagedResponse = new PagedResult<object>(0, 10);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(0, 0)]
    public void PagedResponse_ThrowsErrors_WithInvalidConstructorArgs(int skip, int take)
    {
        var ex = Record.Exception(() => new PagedResult<object>(skip, take));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }

    [Fact]
    public void PagedResponse_Constructs_WithValidConstructorArgs()
    {
        Assert.True(_pagedResponse.HasMore);
        Assert.Equal(0, _pagedResponse.PageNum);
        Assert.Empty(_pagedResponse.Items);

        var ex = Record.Exception(() => _pagedResponse.GetNextPage());

        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Theory, MemberData(nameof(SkipAndTakeTestData))]
    public void PagedResponse_AddsAllItems(int skip, int take)
    {
        _pagedResponse = new PagedResult<object>(skip, take);

        _pagedResponse.OnGetNextPage += (pagedResponse) =>
        {
            pagedResponse.AddItems(_testData
                .Skip(pagedResponse.Skip)
                .Take(pagedResponse.Take));
            return pagedResponse;
        };

        while (_pagedResponse.HasMore)
        {
            _pagedResponse.GetNextPage();
            Assert.Equal((int)Math.Ceiling((decimal)_pagedResponse.Skip / take), _pagedResponse.PageNum);
        }

        Assert.False(_pagedResponse.HasMore);
        for (int i = 0; i < _pagedResponse.Items.Count; i++)
        {
            Assert.Equal(
                _testData[skip + i],
                _pagedResponse.Items[i]);
        }
    }

    [Theory, MemberData(nameof(SkipAndTakeTestData))]
    public void PagedResponse_SetsAllItems(int skip, int take)
    {
        _pagedResponse = new PagedResult<object>(skip, take);

        _pagedResponse.OnGetNextPage += (pagedResponse) =>
        {
            pagedResponse.SetItems(_testData
                .Skip(pagedResponse.Skip)
                .Take(pagedResponse.Take));
            return pagedResponse;
        };

        do
        {
            var lastSkip = _pagedResponse.Skip;
            _pagedResponse.GetNextPage();
            var took = _pagedResponse.Skip - lastSkip;

            Assert.Equal((int)Math.Ceiling((decimal)_pagedResponse.Skip / take), _pagedResponse.PageNum);
            for (int i = 0; i < _pagedResponse.Items.Count; i++)
            {
                Assert.Equal(
                    _testData[_pagedResponse.Skip - took + i],
                    _pagedResponse.Items[i]);
            }
        }
        while (_pagedResponse.HasMore);

        Assert.False(_pagedResponse.HasMore);
    }

    [Theory, MemberData(nameof(SkipAndTakeTestData))]
    public void PageNum_IsCeilingSkipOverTake(int skip, int take)
    {
        _pagedResponse = new PagedResult<object>(skip, take);
        Assert.Equal((int)Math.Ceiling((decimal)skip / take), _pagedResponse.PageNum);
    }
    public static IEnumerable<object[]> SkipAndTakeTestData() =>
    [
        [0, 1],
        [0, 10],
        [0, 17],
        [7, 17],
        [0, 50],
        [13, 50],
        [0, 99],
        [13, 99],
        [0, 100],
        [5, 100],
        [0, 101],
        [16, 101],
        [99, 1],
        [99, 2],
        [100, 1],
        [101, 5]
    ];

    [Fact]
    public void PagedResult_ReturnsNull_WhenHasMoreFalse()
    {
        _pagedResponse = new PagedResult<object>(0, 1);

        _pagedResponse.OnGetNextPage += (pagedResponse) =>
        {
            pagedResponse.SetItems(_testData);
            return pagedResponse;
        };

        _pagedResponse.GetNextPage();
        Assert.False(_pagedResponse.HasMore);
        Assert.Null(_pagedResponse.GetNextPage());
    }

    [Fact]
    public void OnGetNextPage_Throws_WhenSubscriberRemoved()
    {
        _pagedResponse = new PagedResult<object>(0, 1);

        PagedResult<object> GetNextPageHandler(PagedResult<object> pagedResponse)
        {
            pagedResponse.SetItems(_testData);
            return pagedResponse;
        }

        _pagedResponse.OnGetNextPage += GetNextPageHandler;
        _pagedResponse.OnGetNextPage -= GetNextPageHandler;

        var ex = Record.Exception(() => _pagedResponse.GetNextPage());

        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }
}
