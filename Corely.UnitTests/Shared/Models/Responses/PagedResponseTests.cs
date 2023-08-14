using Corely.Shared.Models.Responses;

namespace Corely.UnitTests.Shared.Models.Responses
{
    public class PagedResponseTests
    {
        private readonly PagedResponse<object> pagedResponse;

        public PagedResponseTests()
        {
            pagedResponse = new PagedResponse<object>(0, 10);
        }


    }
}
