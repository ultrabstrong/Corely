dotnet clean Corely.sln --verbosity minimal
dotnet build Corely.sln --verbosity minimal
dotnet test --collect:"XPlat Code Coverage"