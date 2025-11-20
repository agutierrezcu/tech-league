cd .

// dotnet new tool-manifest
// dotnet tool install dotnet-ef

dotnet tool restore

cd ./src/Infrastructure

dotnet ef migrations add FirstMigration -o Database\Migrations

dotnet ef database update