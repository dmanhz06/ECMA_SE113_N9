# Architecture overview

## Observed baseline

The solution contains `src/ECMA.Web/ECMA.Web.csproj`, `src/ECMA.Application/ECMA.Application.csproj`, and `src/ECMA.Domain/ECMA.Domain.csproj`. Common `net10.0`, nullability, and implicit-using settings are in `Directory.Build.props`; there are no PackageReferences or SDK pin. Domain and Application contain no source files because the referenced Phase 1 files were absent during restructuring.

Current request flow is Browser → ASP.NET Core MVC → `HomeController` → Razor Views. `Index`, `Privacy`, and `Error` are template pages, not product features.

`src/ECMA.Web/Program.cs` registers `AddControllersWithViews`, builds the app, configures production exception handling at `/Home/Error` and HSTS, HTTPS redirection, routing, authorization middleware, `MapStaticAssets`, and conventional `{controller=Home}/{action=Index}/{id?}` routing with `.WithStaticAssets()`. Preserve these modern .NET 10 static-asset conventions.

## Implemented Domain/Application baseline

Task 01 implements pure BCL Domain entities, policies, state transitions, and Application use-case services/ports. The aggregate boundaries are `User`, `Event` (tags, timeline, ticket-type configuration), and `Order` (items, tickets, check-in history). Cross-aggregate facts enter policies through use-case parameters or narrow repository queries; Domain never calls a repository. `TicketType.Sold` changes only through the inventory port so Task 02 can provide an atomic PostgreSQL implementation.

Application uses `Result<T>`/`Error`, one `AccessPolicy`, narrow repositories/read queries, `IClock`, `IPasswordHasher`, `ITicketCodeGenerator`, `IOperationLog`, and one narrow `IUnitOfWork`. Read ports return DTOs rather than entities or `IQueryable`. The accepted BR-REG-02 limitation is that per-user count checks do not serialize concurrent orders by the same user.

Browser → MVC Controllers → Application services → Infrastructure adapters/EF Core DbContext → PostgreSQL. Controllers return server-rendered Razor Views; no separate frontend or JSON API exists. Web is the composition root and depends on Application and Infrastructure; Application depends on Domain. Infrastructure owns persistence, seed/reset, clock, password/code/log adapters, and the maintenance worker. Web owns cookie authentication, principal-backed `ICurrentUser`, HTTP result mapping, ViewModels, controllers, and Razor views.

## Cross-cutting concerns

| Concern | Existing implementation | Required direction |
|---|---|---|
| DI | MVC registered in `src/ECMA.Web/Program.cs` | Constructor injection; scoped DbContext/services as appropriate |
| Authentication | None; no authentication registration/middleware | Accepted cookie authentication and database-stored Identity `PasswordHasher<TUser>` hashes; see decision 0002 |
| Authorization | `UseAuthorization`, no protected actions/policies | Single role plus resource ownership/event assignment; immediate role/status changes |
| Validation | Unobtrusive validation partial available; no input forms | ViewModel DataAnnotations, server `ModelState`, service business rules |
| Errors | Non-development exception handler, request ID error ViewModel | Safe errors without stack traces; log unexpected failures |
| Logging | Built-in log-level configuration only | `ILogger<T>`; important operations per NFR-14, no secrets/passwords |
| Time/jobs | None | Testable time source; minute-based idempotent jobs per NFR-11/13 |
| Tests/CI | No test projects, CI, seed/reset scripts | Focused requirement-linked checks; PostgreSQL proof for database concurrency |

The SRS excludes a standalone audit-log feature but requires operational logs and check-in/undo records. Do not confuse these obligations or build a general audit subsystem.

See [product context](../product/agent-context.md) for feature dependencies and unresolved business rules.

Styling direction is Razor with locally compiled Tailwind CSS v4 per [decision 0003](../decisions/0003-razor-tailwind-styling.md). `src/ECMA.Web/package.json` and `src/ECMA.Web/bun.lock` declare the Tailwind CLI packages; the layout still uses Bootstrap and no Tailwind build is configured. These asset dependencies do not change the server-rendered architecture.