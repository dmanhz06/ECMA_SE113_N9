$ErrorActionPreference = 'Stop'
$name = if ($env:POSTGRES_DB) { $env:POSTGRES_DB } else { 'ecma_dev' }
if ($name -notmatch '(?i)(dev|test)') { throw "Refusing to reset database '$name'; its name must contain dev or test." }
$env:ConnectionStrings__ECMA = "Host=$($env:POSTGRES_HOST ?? 'localhost');Port=$($env:POSTGRES_PORT ?? '5432');Database=$name;Username=$($env:POSTGRES_USER);Password=$($env:POSTGRES_PASSWORD)"
dotnet run --project src/ECMA.Web/ECMA.Web.csproj -- --reset-db
