using Corely.DevTools.Attributes;
using Corely.Common.Extensions;

namespace Corely.DevTools.Commands
{
    internal class Url : CommandBase
    {
        [Option("-e", "--encode", Description = "Value to encode")]
        private string Encode { get; init; }

        [Option("-d", "--decode", Description = "Value to decode")]
        private string Decode { get; init; }

        public Url() : base("url", "Url encode/decode operations")
        {
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Encode))
            {
                Console.WriteLine(Encode.UrlEncode());
            }
            if (!string.IsNullOrEmpty(Decode))
            {
                Console.WriteLine(Decode.UrlDecode());
            }
            if (string.IsNullOrEmpty(Encode) && string.IsNullOrEmpty(Decode))
            {
                Console.WriteLine("No value to encode or decode");
            }
        }
    }
}
