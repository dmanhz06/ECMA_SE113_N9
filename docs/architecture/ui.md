# Server-rendered UI

## Existing conventions

- Views live in `src/ECMA.Web/Views/<Controller>/<Action>.cshtml`; shared views/partials live in `src/ECMA.Web/Views/Shared`.
- `src/ECMA.Web/Views/_ViewStart.cshtml` selects `_Layout`; `_ViewImports.cshtml` imports `ECMA.Web`, `ECMA.Web.Models`, and MVC Tag Helpers.
- `_Layout.cshtml` uses `ViewData["Title"]`, `RenderBody`, and optional `Scripts`. Navigation uses `asp-controller`/`asp-action`; the error page uses `@model ErrorViewModel`.
- Tailwind CSS 4.3.3 and `@tailwindcss/cli` are pinned in `src/ECMA.Web/bun.lock`. The source is `src/ECMA.Web/Styles/app.css`; it scans `Views/**/*.cshtml` and emits the committed `wwwroot/css/app.css`. The layout references only that stylesheet.
- From `src/ECMA.Web`, run `bun install --frozen-lockfile` after a clean checkout, `bun run build:css` for a production asset, or `bun run watch:css` during view work. Rebuild and commit `wwwroot/css/app.css` whenever Razor classes or `Styles/app.css` change. Docker integration is intentionally deferred by Task 04.
- Bootstrap assets remain vendored under `wwwroot/lib` but no Razor view references Bootstrap CSS, JS, or classes. jQuery and validation assets remain for unobtrusive client validation.
- Product pages, role-sensitive navigation, error pages, and `<html lang="vi">` use Vietnamese. The system font stack begins with Segoe UI and supports Vietnamese diacritics.

## Feature implementation guidance

Use Razor `.cshtml` and strongly typed `*ViewModel` classes. Add ViewModels only when needed and update imports as appropriate. Use Tailwind CSS v4 for product styling per [decision 0003](../decisions/0003-razor-tailwind-styling.md). No SPA, React, Vue, Angular, Blazor, or separate frontend is implied.

Use complete, statically detectable Tailwind class names. Keep shared element defaults and component classes in `Styles/app.css`; rebuild the committed output after any source or Razor class change. Do not reintroduce Bootstrap references.

Forms should use MVC Tag Helpers (`asp-for`, labels, validation messages/summary), mark required fields, and show errors near fields. Include `_ValidationScriptsPartial` in the `Scripts` section where client validation is useful. Always validate on the server; rebuild selections/data when redisplaying an invalid form. Use antiforgery-protected POST and redirect after success. Bind only editable fields.

Keep forms usable without JavaScript where practical. JavaScript may enhance confirmations and small interactions; it must not carry authorization or business rules. Dangerous actions need confirmation per SRS §4.2. Show operation success/failure on the page; this is allowed feedback, not the excluded notification feature.

Encode descriptions and timeline text through Razor. Preserve Unicode, keyboard navigation, labels, visible focus, readable contrast, and empty/error states. Check supported desktop browsers at ≥1024 px; mobile design is not required. Role-sensitive navigation is helpful but never replaces server checks.

## Optional design skill

The installed `design-taste-frontend` document calls itself `tasteskill` and is intended for landing pages, portfolios, and redesigns, explicitly not dashboards, tables, or multi-step product UI. No exact `taste-skill` folder was found under the inspected repository, user `.agents/skills`, or `.codex/skills` locations.

Use its audit-first approach and typography/contrast/form-state checks selectively for public-facing pages. Do not adopt its React/Next.js/Motion defaults, extra imagery, automatic dark-mode/mobile scope, or design-system dependencies as project requirements. MVC/Razor, the accepted Tailwind choice, and product requirements take priority. Skills are optional and need not be installed to work on ECMA.