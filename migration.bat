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

echo.
set /p MIGRATION_NAME=Enter migration name: 

if "%MIGRATION_NAME%"=="" (
    echo Migration name cannot be empty.
    pause
    exit /b 1
)

echo.
echo Creating migration "%MIGRATION_NAME%"...
echo.

dotnet ef migrations add "%MIGRATION_NAME%" ^
    --project ".\src\ECMA.Infrastructure\ECMA.Infrastructure.csproj" ^
    --startup-project ".\src\ECMA.Web\ECMA.Web.csproj" ^
    --context EcmaDbContext ^
    --output-dir Data\Migrations

if errorlevel 1 (
    echo.
    echo Migration creation failed.
    pause
    exit /b 1
)

echo.
echo Migration "%MIGRATION_NAME%" created successfully.
pause