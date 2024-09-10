## Managing Migrations
Full help can be found at https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

Run `dotnet ef migrations add <MigrationName>` to create a new migration

Run `dotnet ef database update` to apply the migration to the database

Run `dotnet ef migrations remove` to remove the last migration

There are many other commands available, but these are the most common ones you will use.

