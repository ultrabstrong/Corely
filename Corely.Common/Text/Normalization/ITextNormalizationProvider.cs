namespace Corely.Common.Text.Normalization
{
    public interface ITextNormalizationProvider
    {
        string BasicNormalize(string s);

        string NormalizeAddress(string street, params string[] additional);

        string NormalizeAddressAndState(string street, params string[] additional);
    }
}
