@echo off
setlocal
cd /d "%~dp0"

dotnet tool restore
if errorlevel 1 (
    echo.
    echo Restoring local tools failed.
    pause
    exit /b 1
)

echo Applying migrations...

dotnet ef database update ^
    --project ".\src\ECMA.Infrastructure\ECMA.Infrastructure.csproj" ^
    --startup-project ".\src\ECMA.Web\ECMA.Web.csproj" ^
    --context EcmaDbContext

if errorlevel 1 (
    echo.
    echo Applying migrations failed.
    pause
    exit /b 1
)

echo.
echo Migrations applied successfully.
pause