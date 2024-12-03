using System.CommandLine;

namespace Corely.DevTools.Attributes;

internal abstract class AttributeBase : Attribute
{
    public string Description { get; init; } = null;

    public ArgumentArity? ArgumentArity { get; init; }
}
