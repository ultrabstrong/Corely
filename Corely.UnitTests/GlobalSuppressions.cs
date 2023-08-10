// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// member
[assembly: SuppressMessage("Style", "IDE0230:Use UTF-8 string literal", Justification = "BOM reader is a unicorn", Scope = "member", Target = "~M:Corely.UnitTests.Shared.Extensions.BomReaderExtensionsTests.GetEncodingTestData~System.Collections.Generic.IEnumerable{System.Object[]}")]

// module
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking method static just because it doesn't reference local variables provides more confusion than help", Scope = "module")]
