using System.CommandLine;

namespace Corely.DevTools.Attributes
{
    internal abstract class ArgumentAttributeBase : Attribute
    {
        public string Description { get; init; } = null;

        public ArgumentArity? ArgumentArity { get; init; }
    }
}
