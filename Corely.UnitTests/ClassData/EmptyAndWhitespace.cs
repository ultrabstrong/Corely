using System.Collections;

namespace Corely.UnitTests.ClassData
{
    internal class EmptyAndWhitespace : IEnumerable<object[]>
    {
        private readonly List<object[]> _data =
        [
            [""],
            [" "]
        ];
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
