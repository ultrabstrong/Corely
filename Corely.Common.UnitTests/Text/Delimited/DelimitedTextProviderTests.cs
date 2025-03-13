using Corely.Common.Text.Delimited;
using Corely.TestBase;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;

namespace Corely.Common.UnitTests.Text.Delimited;

public class DelimitedTextProviderTests
{
    private readonly ILogger<DelimitedTextProvider> _logger;
    private readonly DelimitedTextProvider _delimitedTextDataProvider;

    public DelimitedTextProviderTests()
    {
        _logger = new NullLogger<DelimitedTextProvider>();
        _delimitedTextDataProvider = new DelimitedTextProvider(_logger);
    }

    [Theory]
    [InlineData(TokenDelimiter.Semicolon, ';', '"', "\r\n")]
    [InlineData(TokenDelimiter.Pipe, '|', '"', "\r\n")]
    [InlineData(TokenDelimiter.Tab, '\t', '"', "\r\n")]
    [InlineData(TokenDelimiter.Comma, ',', '"', "\r\n")]
    public void Constructor_CreatesCorrectDelimiters_ForEnum(
        TokenDelimiter tokenDelimiter,
        char expectedTokenDelim,
        char expectedTokenLiteral,
        string expectedRecordDelim)
    {
        DelimitedTextProvider delimitedTextDataProvider = new(_logger, tokenDelimiter);

        var tokenDelim = NonPublicHelpers.GetNonPublicField<char>(
            delimitedTextDataProvider, "_tokenDelimiter");

        var tokenLiteral = NonPublicHelpers.GetNonPublicField<char>(
            delimitedTextDataProvider, "_tokenLiteral");

        var recordDelim = NonPublicHelpers.GetNonPublicField<string>(
            delimitedTextDataProvider, "_recordDelimiter");

        Assert.Equal(expectedTokenDelim, tokenDelim);
        Assert.Equal(expectedTokenLiteral, tokenLiteral);
        Assert.Equal(expectedRecordDelim, recordDelim);
    }

    [Fact]
    public void WriteRecord_WritesCorrectRecord()
    {
        string expected = "test1,te\"\"st2,\"te,st3\"";
        string actual;
        using (MemoryStream stream = new())
        {
            _delimitedTextDataProvider.WriteRecord(["test1", "te\"st2", "te,st3"], stream);
            stream.Position = 0;
            using (StreamReader reader = new(stream))
            {
                actual = reader.ReadToEnd();
            }
        }

        Assert.Equal(expected, actual);
    }

    [Theory, MemberData(nameof(WriteAllRecordsTestData))]
    public void WriteAllRecordsThenReadAllRecords_ProducesOriginalInput(List<List<string>> records)
    {
        string delimitedData;
        using (MemoryStream stream = new())
        {
            _delimitedTextDataProvider.WriteAllRecords(records, stream);
            stream.Position = 0;
            using (StreamReader reader = new(stream))
            {
                delimitedData = reader.ReadToEnd();
            }
        }

        List<ReadRecordResult> results = [];
        using (MemoryStream stream = new(Encoding.UTF8.GetBytes(delimitedData)))
        {
            results = _delimitedTextDataProvider.ReadAllRecords(stream);
        }

        Assert.Equal(records.Count, results.Count);
        for (int i = 0; i < records.Count; i++)
        {
            Assert.Equal(records[i].Count, results[i].Tokens.Count);
            for (int j = 0; j < records[i].Count; j++)
            {
                Assert.Equal(records[i][j], results[i].Tokens[j]);
            }
        }
    }

    public static IEnumerable<object[]> WriteAllRecordsTestData() =>
    [
        // No delimiters in tokens
        [new List<List<string>> {
                new() { "test1", "test2", "test3" }
            }],

            // One delimiter type in tokens
            [new List<List<string>>() {
                //// Token literal
                new() { "\"test1", "\"test2", "\"test3" },
                new() { "test1\"", "test2\"", "test3\"" },
                new() { "te\"st1", "te\"st2", "te\"st3" },

                // Token delimiter
                new() { ",test1", ",test2", ",test3" },
                new() { "test1,", "test2,", "test3," },
                new() { "te,st1", "te,st2", "te,st3" },

                // Record delimiter
                new() { "\r\ntest1", "\r\ntest2", "\r\ntest3" },
                new() { "test1\r\n", "test2\r\n", "test3\r\n" },
                new() { "te\r\nst1", "te\r\nst2", "te\r\nst3" },
            }],

            // Mixed delimiter types in tokens
            [new List<List<string>>() {
                // Token literal and token delimiter

                    // Token literal first
                new() { "\",test1", "\",test2", "\",test3" },
                new() { "test1\",", "test2\",", "test3\"," },
                new() { "te\",st1", "te\",st2", "te\",st3" },

                    // Token delimiter first
                new() { ",\"test1", ",\"test2", ",\"test3" },
                new() { "test1,\"", "test2,\"", "test3,\"" },
                new() { "te,\"st1", "te,\"st2", "te,\"st3" },

                // Token literal and record delimiter

                    // Token literal first
                new() { "\"\r\ntest1", "\"\r\ntest2", "\"\r\ntest3" },
                new() { "test1\"\r\n", "test2\"\r\n", "test3\"\r\n" },
                new() { "te\"\r\nst1", "te\"\r\nst2", "te\"\r\nst3" },

                    // Record delimiter first
                new() { "\r\n\"test1", "\r\n\"test2", "\r\n\"test3" },
                new() { "test1\"\r\n", "test2\"\r\n", "test3\"\r\n" },
                new() { "te\"\r\nst1", "te\"\r\nst2", "te\"\r\nst3" },

                // Token delimiter and record delimiter

                    // Token delimiter first
                new() { ",\r\ntest1", ",\r\ntest2", ",\r\ntest3" },
                new() { "test1,\r\n", "test2,\r\n", "test3,\r\n" },
                new() { "te,\r\nst1", "te,\r\nst2", "te,\r\nst3" },
                    // Record delimiter first
                new() { "\r\n,test1", "\r\n,test2", "\r\n,test3" },
                new() { "test1\r\n,", "test2\r\n,", "test3\r\n," },
                new() { "te\r\nst1,", "te\r\n,st2", "te\r\n,st3" },
            }],

            // Mixed delimiter types in tokens with excess token literals
            [new List<List<string>>() {
                // Token literal (2x) and token delimiter

                    // Token literal first
                new() { "\"\",test1", "\"\",test2", "\"\",test3" },
                new() { "test1\"\",", "test2\"\",", "test3\"\"," },
                new() { "te\"\",st1", "te\"\",st2", "te\"\",st3" },

                    // Token delimiter first
                new() { ",\"\"test1", ",\"\"test2", ",\"\"test3" },
                new() { "test1,\"\"", "test2,\"\"", "test3,\"\"" },
                new() { "te,\"\"st1", "te,\"\"st2", "te,\"\"st3" },

                // Token literal (2x) and record delimiter

                    // Token literal first
                new() { "\"\"\r\ntest1", "\"\"\r\ntest2", "\"\"\r\ntest3" },
                new() { "test1\"\"\r\n", "test2\"\"\r\n", "test3\"\"\r\n" },
                new() { "te\"\"\r\nst1", "te\"\"\r\nst2", "te\"\"\r\nst3" },

                    // Record delimiter first
                new() { "\r\n\"\"test1", "\r\n\"\"test2", "\r\n\"\"test3" },
                new() { "test1\"\"\r\n", "test2\"\"\r\n", "test3\"\"\r\n" },
                new() { "te\"\"\r\nst1", "te\"\"\r\nst2", "te\"\"\r\nst3" },
                    
                // Token literal (3x) and token delimiter

                    // Token literal first
                new() { "\"\"\",test1", "\"\"\",test2", "\"\"\",test3" },
                new() { "test1\"\"\",", "test2\"\"\",", "test3\"\"\"," },
                new() { "te\"\"\",st1", "te\"\"\",st2", "te\"\"\",st3" },

                    // Token delimiter first
                new() { ",\"\"\"test1", ",\"\"\"test2", ",\"\"\"test3" },
                new() { "test1,\"\"\"", "test2,\"\"\"", "test3,\"\"\"" },
                new() { "te,\"\"\"st1", "te,\"\"\"st2", "te,\"\"\"st3" },

                // Token literal (3x) and record delimiter

                    // Token literal first
                new() { "\"\"\"\r\ntest1", "\"\"\"\r\ntest2", "\"\"\"\r\ntest3" },
                new() { "test1\"\"\"\r\n", "test2\"\"\"\r\n", "test3\"\"\"\r\n" },
                new() { "te\"\"\"\r\nst1", "te\"\"\"\r\nst2", "te\"\"\"\r\nst3" },

                    // Record delimiter first
                new() { "\r\n\"\"\"test1", "\r\n\"\"\"test2", "\r\n\"\"\"test3" },
                new() { "test1\"\"\"\r\n", "test2\"\"\"\r\n", "test3\"\"\"\r\n" },
                new() { "te\"\"\"\r\nst1", "te\"\"\"\r\nst2", "te\"\"\"\r\nst3" },
            }],

            // All three delimiter types
            [new List<List<string>>() {
                new() { "\",test1\r\ntest1.1,\"", "test2", "test3" },
                new() { "test1", "\",test2\r\ntest2.1,\"", "test3" },
                new() { "test1", "test2", "\",test3\r\ntest3.1,\"" }
            }],
        ];
}

