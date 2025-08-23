using Corely.IAM.Mappers;
using Corely.IAM.Validation;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Processors;

public abstract class ProcessorBase
{
    private readonly IMapProvider _mapProvider;
    private readonly IValidationProvider _validationProvider;
    private readonly ILogger _logger;

    protected ProcessorBase(
        IMapProvider mapProvider,
        IValidationProvider validationProvider,
        ILogger logger)
    {
        _mapProvider = mapProvider;
        _validationProvider = validationProvider;
        _logger = logger;
    }

    protected TDestination? MapTo<TDestination>(object? source)
    {
        return _mapProvider.MapTo<TDestination>(source);
    }

    protected TDestination MapThenValidateTo<TDestination>(object source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var mapped = _mapProvider.MapTo<TDestination>(source);
        if (mapped != null)
        {
            _validationProvider.ValidateAndThrow(mapped);
        }
        return mapped!;
    }
}