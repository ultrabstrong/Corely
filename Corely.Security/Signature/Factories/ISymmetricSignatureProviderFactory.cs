using Corely.Security.Signature.Providers;

namespace Corely.Security.Signature.Factories;

public interface ISymmetricSignatureProviderFactory : IProviderFactory<ISymmetricSignatureProvider>
{
    ISymmetricSignatureProvider GetProviderForVerifying(string value);
}
