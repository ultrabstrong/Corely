using Corely.Common.Models.Results;

namespace Corely.UnitTests.Common.Models.Results
{
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
        public void PagedResponse_ShouldThrowErrors_WithInvalidConstructorArgs(int skip, int take)
        {
            var ex = Record.Exception(() => new PagedResult<object>(skip, take));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        public void PagedResponse_ShouldConstruct_WithValidConstructorArgs()
        {
            Assert.True(_pagedResponse.HasMore);
            Assert.Equal(0, _pagedResponse.PageNum);
            Assert.Empty(_pagedResponse.Items);

            var ex = Record.Exception(() => _pagedResponse.GetNextChunk());

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Theory, MemberData(nameof(SkipAndTakeTestData))]
        public void PagedResponse_ShouldAddAllItems(int skip, int take)
        {
            _pagedResponse = new PagedResult<object>(skip, take);

            _pagedResponse.OnGetNextChunk += (pagedResponse) =>
            {
                pagedResponse.AddItems(_testData
                    .Skip(pagedResponse.Skip)
                    .Take(pagedResponse.Take));
                return pagedResponse;
            };

            while (_pagedResponse.HasMore)
            {
                _pagedResponse.GetNextChunk();
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
        public void PagedResponse_ShouldSetAllItems(int skip, int take)
        {
            _pagedResponse = new PagedResult<object>(skip, take);

            _pagedResponse.OnGetNextChunk += (pagedResponse) =>
            {
                pagedResponse.SetItems(_testData
                    .Skip(pagedResponse.Skip)
                    .Take(pagedResponse.Take));
                return pagedResponse;
            };

            do
            {
                var lastSkip = _pagedResponse.Skip;
                _pagedResponse.GetNextChunk();
                var took = _pagedResponse.Skip - lastSkip;

                Assert.Equal((int)Math.Ceiling((decimal)_pagedResponse.Skip / take), _pagedResponse.PageNum);
                for (int i = 0; i < _pagedResponse.Items.Count; i++)
                {
                    Assert.Equal(
                        _testData[(_pagedResponse.Skip - took) + i],
                        _pagedResponse.Items[i]);
                }
            }
            while (_pagedResponse.HasMore);

            Assert.False(_pagedResponse.HasMore);
        }

        [Theory, MemberData(nameof(SkipAndTakeTestData))]
        public void PageNum_ShouldBeCeilingSkipOverTake(int skip, int take)
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
        public void PagedResult_ShouldReturnNull_WhenHasMoreFalse()
        {
            _pagedResponse = new PagedResult<object>(0, 1);

            _pagedResponse.OnGetNextChunk += (pagedResponse) =>
            {
                pagedResponse.SetItems(_testData);
                return pagedResponse;
            };

            _pagedResponse.GetNextChunk();
            Assert.False(_pagedResponse.HasMore);
            Assert.Null(_pagedResponse.GetNextChunk());
        }

        [Fact]
        public void OnGetNextChunk_ShouldThrow_WhenSubscriberRemoved()
        {
            _pagedResponse = new PagedResult<object>(0, 1);

            PagedResult<object> GetNextChunkHandler(PagedResult<object> pagedResponse)
            {
                pagedResponse.SetItems(_testData);
                return pagedResponse;
            }

            _pagedResponse.OnGetNextChunk += GetNextChunkHandler;
            _pagedResponse.OnGetNextChunk -= GetNextChunkHandler;

            var ex = Record.Exception(() => _pagedResponse.GetNextChunk());

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }
    }
}
