using System.Collections;

namespace Corely.UnitTests.ClassData
{
    internal class NullAndEmpty : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new()
        {
            new object[] { null },
            new object[] { "" },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
