using Corely.Security.Signature.Providers;

namespace Corely.Security.Signature.Factories;

public interface IAsymmetricSignatureProviderFactory : IProviderFactory<IAsymmetricSignatureProvider>
{
    IAsymmetricSignatureProvider GetProviderForVerifying(string value);
}
