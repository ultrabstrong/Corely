using Corely.Shared.Extensions;
using Corely.Shared.Providers.Data;
using Corely.Shared.Providers.Data.Models;
using System.Text;

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
        public void ReadAllRecordsThenWriteAllRecords_ShouldProduceOriginalInput()
        {
            string base64EncodedTestData = "dGVzdDEsdGVzdDIsdGVzdDMNCiIiInRlc3QxIiwiIiJ0ZXN0MiIsIiIidGVzdDMiDQoidGVzdDEiIiIsInRlc3QyIiIiLCJ0ZXN0MyIiIg0KInRlIiJzdDEiLCJ0ZSIic3QyIiwidGUiInN0MyINCiIsdGVzdDEiLCIsdGVzdDIiLCIsdGVzdDMiDQoidGVzdDEsIiwidGVzdDIsIiwidGVzdDMsIg0KInRlLHN0MSIsInRlLHN0MiIsInRlLHN0MyINCiIKdGVzdDEiLHRlc3QyLHRlc3QzDQoidGVzdDEKIix0ZXN0Mix0ZXN0Mw0KdGVzdDEsIgp0ZXN0MiIsdGVzdDMNCnRlc3QxLCJ0ZXN0MgoiLHRlc3QzDQp0ZXN0MSx0ZXN0MiwiCnRlc3QzIg0KdGVzdDEsdGVzdDIsInRlc3QzCiINCiIiIix0ZXN0MSwKdGVzdDEuMSwiIiIsdGVzdDIsdGVzdDMNCnRlc3QxLCIiIix0ZXN0MiwKdGVzdDIuMiwiIiIsdGVzdDMNCnRlc3QxLHRlc3QyLCIiIix0ZXN0MywKdGVzdDMuMywiIiI=";
            string testData = base64EncodedTestData.Base64Decode();

            List<ReadRecordResult> results = new List<ReadRecordResult>();
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testData)))
            {
                results = _delimitedTextDataProvider.ReadAllRecords(stream);
            }

            StringBuilder sb = new StringBuilder();
            foreach (ReadRecordResult result in results)
            {
                foreach (var s in result.Tokens)
                {
                    if (s.Contains(',') | s.Contains('"') | s.Contains(Environment.NewLine))
                    {
                        sb.Append('"');
                        sb.Append(s);
                        sb.Append('"');
                    }
                    else
                    {
                        sb.Append(s);
                    }
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }

            string asdf = sb.ToString();

            string resultData;
            using (MemoryStream stream = new MemoryStream())
            {
                _delimitedTextDataProvider.WriteAllRecords(results.Select(m => m.Tokens), stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    resultData = reader.ReadToEnd();
                }
            }
            string base64EncodedResultData = resultData.Base64Encode();

            Assert.Equal(base64EncodedTestData, base64EncodedResultData);
        }
    }
}
