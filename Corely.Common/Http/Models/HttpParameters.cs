namespace Corely.Common.Http.Models;

public sealed class HttpParameters : HttpParametersBase
{
    public HttpParameters()
        : base()
    {
    }

    public HttpParameters(string key, string value)
        : base(key, value)
    {
    }

    public HttpParameters(Dictionary<string, string> parameters)
        : base(parameters)
    {
    }

    public HttpParameters(Dictionary<string, string> parameters, Dictionary<string, string> tempParameters)
        : base(parameters, tempParameters)
    {
    }

    public override void ValidateParameter(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));
    }
}
