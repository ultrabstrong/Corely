using Corely.DevTools.Attributes;
using Corely.Shared.Extensions;

namespace Corely.DevTools.Commands
{
    internal class Base64 : CommandBase
    {
        [Option("-e", "--encode", Description = "Value to encode")]
        private string Encode { get; init; }

        [Option("-d", "--decode", Description = "Value to decode")]
        private string Decode { get; init; }

        public Base64() : base("base64", "Base64 operations")
        {
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Encode))
            {
                Console.WriteLine(Encode.Base64Encode());
            }
            if (!string.IsNullOrEmpty(Decode))
            {
                Console.WriteLine(Decode.Base64Decode());
            }
            if (string.IsNullOrEmpty(Encode) && string.IsNullOrEmpty(Decode))
            {
                Console.WriteLine("No value to encode or decode");
            }
        }
    }
}
