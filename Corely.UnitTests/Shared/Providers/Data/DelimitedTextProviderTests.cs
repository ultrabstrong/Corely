using Corely.Shared.Providers.Data;

namespace Corely.UnitTests.Shared.Providers.Data
{
    public class DelimitedTextProviderTests
    {
        private readonly DelimitedTextProvider _delimitedTextDataProvider;

        public DelimitedTextProviderTests()
        {
            _delimitedTextDataProvider = new DelimitedTextProvider();
        }

        [Fact]
        public void Tests_ShouldBeImplemented_AtSomePointInTheFuture()
        {
            /*
             * 8-10-23
             * I spent a siginifcant amount of time thinking of how to auto-generate tests for this class.
             * After much consideration I realized there are potentially millions of test cases that could be generated.
             * This is obviously not feasible or helpful for unit testing
             * 
             * The best thing to do is hand code the most likely "gotchya" test cases
             * The tests should focus on token literals (i.e. tokens that contain delimiters.)
             * 
             * We ideally want to test:
             * - each type of delimiter and combinations of delimiters
             * - delimiters at/near beginning and at/near end of token
             * - combinations of delimiters at/near beginning and at/near end of token
             * - delimiters at/near beginning and at/near end of record
             * - combinations of delimiters at/near beginning and at/near end of record
             * 
             * It's probably easy to go overboard even with handwriting tests. Work on real-world cases first,
             * like types of thing's I've actually see in a delimited file:
             * - tokens that have token literal delimiters (beginning, middle, and end of token)
             * - records that have tokens with token literal delimiters (beginning and end of record)
             * - tokens that have token and/or record delimiters (beginning, middle, and end of token)
             *     - consider empty beginning and end case
             *     - consider these at beginning and end of records
             *     - consider these at beginning and end of file
             *     - consider there might be tokens that are themselves lists of records that use the same delimiters as the record they're in.
             * 
             * Its late; I'm tired; and this is not a high priority right now.
             * Hopefully one day I'll have a reason to come back and finish this, but for now there are bigger fish to fry.
             */

            // use it to suppress IDE0052 suggestion
            _delimitedTextDataProvider.GetType();
            Assert.True(true);
        }
    }
}
