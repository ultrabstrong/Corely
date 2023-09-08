using Corely.DevTools.Attributes;

namespace Corely.DevTools.Commands
{
    internal class AesKey : CommandBase
    {
        [Option("-v", "--validate", Description = "Validate a key")]
        private string? Validate { get; init; }

        public AesKey() : base("aes-key", "Creates a new AES key")
        {
        }

        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(Validate))
            {
                CreatKey();
            }
            else
            {
                ValidateKey();
            }
        }

        private void CreatKey()
        {
            var key = new Corely.Shared.Providers.Security.AesKeyProvider().CreateKey();
            Console.WriteLine($"Key: {key}");
        }

        private void ValidateKey()
        {
            var isValid = new Corely.Shared.Providers.Security.AesKeyProvider().IsKeyValid(Validate);
            Console.WriteLine($"Key is {(isValid ? "valid" : "invalid")}");
        }
    }
}
