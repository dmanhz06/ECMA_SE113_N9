# Persistence

## Current state

PostgreSQL and EF Core 10 are implemented in `src/ECMA.Infrastructure`. `Data/EcmaDbContext.cs` owns the context, `Data/Configurations.cs` owns one Fluent API configuration per entity, `Data/Repositories.cs` and `Data/Queries.cs` implement Application ports, and `Data/Migrations` contains the reviewed initial migration. `src/ECMA.Web/Program.cs` calls `AddInfrastructure`.

The connection-string key is `ConnectionStrings:ECMA`; provide it through environment variables or user secrets. For direct non-container development runs, the Web startup also reads `ECMA_CONNECTION_STRING` from the nearest `.env` found at or above its content root, but only when `ConnectionStrings:ECMA` is otherwise unset. Credentials are not committed. Repository-local `dotnet-ef` 10.0.4 is pinned in `.config/dotnet-tools.json`. The generated idempotent SQL is saved at `docs/database/initial.sql`.

`compose.yaml` runs pinned `postgres:18.3-alpine` with a healthcheck and named volume. Copy `.env.example` to untracked `.env`, replace the password, then run `docker compose up -d postgres`. Docker Compose uses `ECMA_CONNECTION_STRING` when present and otherwise connects Web to its `postgres` service. Production never applies migrations automatically.

Demo seeding uses `dotnet run --project src/ECMA.Web/ECMA.Web.csproj -- --seed`. It requires `Seed:AdminUsername`, `Seed:AdminPassword`, and `Seed:DemoPassword` from configuration. Development-only example values are `admin_user`, `AdminPass123`, and `DemoPass123`; never reuse them outside disposable local databases. Reset uses `scripts/reset-db.ps1` or `scripts/reset-db.sh` and refuses database names without `dev` or `test`.

## Data invariants

- Instants (event/sale/check-in/expiry/creation times) are stored as UTC and presented as Asia/Ho_Chi_Minh (UTC+7). Birth dates are calendar dates, not UTC instants. Exact CLR/store types remain undecided.
- VND amounts are integers. Choose a type that can hold totals, not only the 100,000,000 VND ticket-price maximum. Capture purchase unit prices for history/reporting.
- The SRS specifies lowercase usernames with case-insensitive uniqueness/login, random globally unique `TKT-` codes, fixed tags, and unique EventTag/EventStaff pairs. Enforce relevant invariants in the database, not only application checks.
- SQL table/column casing, key types, enum representation, and deletion/cascade conventions are not established. The SRS snake_case property examples are not proof of implemented EF naming.
- Review required/optional fields, FKs, unique constraints, query indexes, capacity accounting, delete behavior, and concurrency for every schema change. Event deletion is soft deletion per FR-EVT-07.
- NFR-08 requires transactional Order/Ticket/inventory operations; BR-REG-04 requires no overselling under concurrent requests. An EF transaction alone does not prove race safety. Choose and test locking/conditional updates/concurrency controls against PostgreSQL, including Pending holds and release/idempotency paths.

## Migration workflow (conditional, not runnable yet)

Only after packages, tools, DbContext, and connection configuration exist, run from the root. Replace `FeatureName` with a meaningful migration name; these commands assume the recommended paths/context were adopted. Otherwise derive arguments from the implemented context.

```text
dotnet tool restore
dotnet ef migrations add FeatureName --project src/ECMA.Infrastructure/ECMA.Infrastructure.csproj --startup-project src/ECMA.Web/ECMA.Web.csproj --context EcmaDbContext --output-dir Data/Migrations
dotnet ef migrations script --idempotent --project src/ECMA.Infrastructure/ECMA.Infrastructure.csproj --startup-project src/ECMA.Web/ECMA.Web.csproj --context EcmaDbContext
dotnet build ECMA.sln
dotnet test ECMA.sln
dotnet ef database update --project src/ECMA.Infrastructure/ECMA.Infrastructure.csproj --startup-project src/ECMA.Web/ECMA.Web.csproj --context EcmaDbContext
```

`dotnet tool restore` requires the future local manifest. Inspect migration code, snapshot, and SQL before application. Apply only to an authorized development database; check the command result and database migration history/state. If PostgreSQL is unavailable, report application/integration verification as blocked. Never equate migration generation with application. Back up and obtain approval for destructive changes; do not edit migrations already shared/applied to bypass history. Document a safe rollback or forward-fix plan for significant schema changes.