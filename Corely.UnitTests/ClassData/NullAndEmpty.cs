﻿using System.Collections;

namespace Corely.UnitTests.ClassData
{
    internal class NullAndEmpty : IEnumerable<object[]>
    {
        private readonly List<object[]> _data =
        [
            [null],
            [""],
        ];

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
