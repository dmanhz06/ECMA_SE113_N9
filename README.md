# ECMA — Events and Conferences Management Application

ECMA is a desktop-browser web application for organizing events and conferences, registering attendees, managing tickets, and tracking attendance.

The goal is a minimal, usable event-management system with clear business rules that support requirement-based testing: boundary values, equivalence partitioning, state transitions, and decision tables.

## Intended capabilities

| User | Planned capabilities |
|---|---|
| Guest | Browse and search public events; view event details, timelines, and ticket types. |
| Attendee | Register and sign in, manage a profile, order tickets, complete mock payments, view tickets and a personal schedule, and cancel eligible registrations. |
| Organizer | Create and publish events, manage timelines and ticket inventory, manage participants, assign check-in staff, and view event reports. |
| Staff | Check in attendees and undo check-ins for assigned events. |
| Admin | Manage users, roles, and account status; oversee events and system reporting. |

Conferences use the same workflow as events: a conference is an event tagged **Conference**, not a separate event type.

## Scope and conventions

- Payments and refunds are simulated; no real payment gateway is used.
- Accounts are identified by username, not email.
- Email and notifications are excluded. Users view statuses and operation feedback in the application.
- Access is role-scoped, with event ownership and staff assignments enforced on the server.
- Times are stored in UTC and displayed in **Asia/Ho_Chi_Minh (UTC+7)**.
- Monetary amounts are integer **VND** values.
- The target is current desktop Chrome, Firefox, and Edge, with a minimum viewport width of 1024 px.

## Technology and current status

| Area | Technology / status |
|---|---|
| Runtime | .NET 10 and ASP.NET Core MVC |
| Rendering | Server-rendered Razor (`.cshtml`); no separate SPA is required |
| Database | PostgreSQL with EF Core 10 / Npgsql, migrations and repositories under `src/ECMA.Infrastructure` |
| Authentication | Cookie authentication, password hashing, active-account validation and role-aware application services |
| Styling | Razor with Tailwind CSS v4 source and committed compiled asset; rebuild using Bun after view/style changes |
| Testing | xUnit test projects under `tests/`; execution requires the .NET 10 SDK |

Accepted decisions take precedence over conflicting SRS wording: the application uses MVC/Razor rather than a separate frontend/REST API, and Identity PBKDF2 password hashing rather than bcrypt/argon2.

## Run locally

Prerequisite: a **.NET 10 SDK**. Run these commands from the repository root:

```shell
dotnet restore ECMA.sln
dotnet build ECMA.sln --no-restore
dotnet run --project src/ECMA.Web/ECMA.Web.csproj --launch-profile http --no-build
```

Open **http://localhost:5054**. Database-backed features require a configured PostgreSQL connection.

For a local PostgreSQL instance, copy `.env.example` to `.env`, replace the example passwords, and start the database with `docker compose up -d postgres`. The Web startup reads `ECMA_CONNECTION_STRING` from `.env` for direct local runs if `ConnectionStrings:ECMA` is unset. Alternatively set `ConnectionStrings__ECMA` in the environment or use .NET user secrets.

Apply reviewed migrations to the intended development database:

```shell
dotnet tool restore
dotnet ef database update --project src/ECMA.Infrastructure/ECMA.Infrastructure.csproj --startup-project src/ECMA.Web/ECMA.Web.csproj --context EcmaDbContext
```

To seed a development administrator, configure `SEED_ADMIN_USERNAME`, `SEED_ADMIN_FULL_NAME`, and `SEED_ADMIN_PASSWORD`, then run:

```shell
dotnet run --project src/ECMA.Web/ECMA.Web.csproj -- --seed
```

Do not reuse development seed passwords outside disposable local environments.

For frontend asset changes, install [Bun](https://bun.sh/), then from `src/ECMA.Web` run `bun install --frozen-lockfile` and `bun run build:css`.

## Task 14 — Event Statistics

The overall event statistics screen is available at `/events/manage/statistics` for active Organizer and Admin accounts. Organizer statistics are scoped to their own events; Admin statistics cover all non-soft-deleted events. The per-event statistics in participant management remain in place. This overview is an explicit Task 14 extension to SRS §1 / §3.6, which require per-event statistics and do not require a standalone dashboard. See [Task 14 handover](docs/development/task14-handover.md) for formulas, implementation notes, and verification limitations.

## Verification

```shell
dotnet restore ECMA.sln
dotnet build ECMA.sln --no-restore
dotnet test ECMA.sln
```

## Repository guide

- `src/ECMA.Web/` — MVC application, Razor views, configuration, static assets, and frontend dependencies.
- `src/ECMA.Application/` — Application boundary; currently an empty project referencing Domain.
- `src/ECMA.Domain/` — Domain boundary; currently an empty dependency-free project.
- `tests/` — Solution folder for future test projects; no test projects currently exist.
- `docs/` — Documentation folder.
- `docs/product/` — requirements, feature dependencies, and open questions.
- `docs/decisions/` — accepted technology and architecture decisions.
- `docs/architecture/` — architecture, persistence, and UI guidance.
- `docs/development/` — conventions and verification workflow.

## Documentation

- [Software Requirements Specification](docs/product/SRS_ECMA.md) — detailed functional requirements, business rules, and non-functional requirements.
- [Product context](docs/product/agent-context.md) — feature map and unresolved decisions.
- [Architecture overview](docs/architecture/overview.md).
- [Accepted decisions](docs/decisions/README.md).
- [Development conventions](docs/development/conventions.md).
- [Verification guide](docs/development/verification.md).