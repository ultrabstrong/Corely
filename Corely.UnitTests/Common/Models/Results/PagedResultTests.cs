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

        [Fact]
        public void PagedResponse_ShouldThrowErrors_WithInvalidConstructorArgs()
        {
            void act1() => new PagedResult<object>(-1, 10);
            Assert.Throws<ArgumentOutOfRangeException>(act1);
            void act2() => new PagedResult<object>(0, 0);
            Assert.Throws<ArgumentOutOfRangeException>(act2);
        }

        [Fact]
        public void PagedResponse_ShouldConstruct_WithValidConstructorArgs()
        {
            Assert.True(_pagedResponse.HasMore);
            Assert.Equal(0, _pagedResponse.PageNum);
            Assert.Empty(_pagedResponse.Items);
            Assert.Throws<InvalidOperationException>(_pagedResponse.GetNextChunk);
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
        public static IEnumerable<object[]> SkipAndTakeTestData()
        {
            yield return new object[] { 0, 1 };
            yield return new object[] { 0, 10 };
            yield return new object[] { 0, 17 };
            yield return new object[] { 7, 17 };
            yield return new object[] { 0, 50 };
            yield return new object[] { 13, 50 };
            yield return new object[] { 0, 99 };
            yield return new object[] { 13, 99 };
            yield return new object[] { 0, 100 };
            yield return new object[] { 5, 100 };
            yield return new object[] { 0, 101 };
            yield return new object[] { 16, 101 };
            yield return new object[] { 99, 1 };
            yield return new object[] { 99, 2 };
            yield return new object[] { 100, 1 };
            yield return new object[] { 101, 5 };
        }

    }
}
