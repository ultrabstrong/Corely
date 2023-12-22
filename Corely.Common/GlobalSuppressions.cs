// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Private variables should begin with underscore", Scope = "module")]
[assembly: SuppressMessage("Performance", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.", Justification = "This is less clear than just using Regex", Scope = "module")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking method static just because it doesn't reference local variables provides more confusion than help", Scope = "module")]
