using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using ECMA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECMA.Infrastructure.Repositories;

public sealed class EventRepository(EcmaDbContext context) : IEventRepository
{
    public async Task<Event?> FindAsync(Guid id, bool forUpdate, CancellationToken cancellationToken = default)
    {
        if (forUpdate)
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT id FROM events WHERE id = {id} AND NOT is_deleted FOR UPDATE", cancellationToken);
        return await context.Events.Include(x => x.AgendaItems).SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> ListAsync(EventQuery query, DateTime now, CancellationToken cancellationToken = default)
    {
        IQueryable<Event> items = context.Events.AsNoTracking().Where(x => !x.IsDeleted);
        if (query.Role != UserRole.Admin) items = items.Where(x => x.OwnerId == query.ActorId);
        if (query.Keyword is not null) items = items.Where(x => EF.Functions.ILike(x.Title, $"%{query.Keyword}%"));
        if (query.Type is not null) items = items.Where(x => x.Type == query.Type);
        if (query.Status is EffectiveEventStatus.Completed) items = items.Where(x => x.Status == EventStatus.Published && x.EndAt <= now);
        else if (query.Status is EffectiveEventStatus.Published) items = items.Where(x => x.Status == EventStatus.Published && x.EndAt > now);
        else if (query.Status is not null) items = items.Where(x => x.Status == (EventStatus)query.Status);
        return await items.OrderByDescending(x => x.CreatedAt).ThenBy(x => x.Id).Skip((query.Page - 1) * 20).Take(20).ToListAsync(cancellationToken);
    }

    // BR-SRCH-01/02: published events that have not ended, ordered by start then ID.
    public async Task<IReadOnlyList<PublicEventRow>> SearchPublicAsync(PublicEventQuery query, DateTime now, CancellationToken cancellationToken = default)
    {
        var items = context.Events.AsNoTracking().Where(x => !x.IsDeleted && x.Status == EventStatus.Published && x.EndAt > now);
        if (query.Keyword is not null)
        {
            // Substring match without user wildcards: Contains is translated to strpos, not LIKE.
            var keyword = query.Keyword.ToLowerInvariant();
            items = items.Where(x => x.Title.ToLower().Contains(keyword) || x.Location.ToLower().Contains(keyword));
        }
        if (query.Type is not null) items = items.Where(x => x.Type == query.Type);
        if (query.StartFrom is not null) items = items.Where(x => x.StartAt >= query.StartFrom);
        if (query.StartBefore is not null) items = items.Where(x => x.StartAt < query.StartBefore);
        var rows = await items.OrderBy(x => x.StartAt).ThenBy(x => x.Id).Skip((query.Page - 1) * 20).Take(20)
            .Select(x => new { Event = x, Confirmed = context.Registrations.Count(r => r.EventId == x.Id && r.Status == RegistrationStatus.Confirmed) })
            .ToListAsync(cancellationToken);
        return rows.Select(x => new PublicEventRow(x.Event, x.Confirmed)).ToList();
    }


    public async Task<IReadOnlyList<EventStatisticsRecord>> GetStatisticsAsync(Guid? ownerId, DateTime now, CancellationToken cancellationToken = default)
    {
        // Aggregate registrations in PostgreSQL before joining to events; no Registration entities are materialized.
        var counts = context.Registrations.AsNoTracking()
            .GroupBy(r => r.EventId)
            .Select(g => new
            {
                EventId = g.Key,
                Confirmed = g.Count(r => r.Status == RegistrationStatus.Confirmed),
                Cancelled = g.Count(r => r.Status == RegistrationStatus.Cancelled),
                CheckedIn = g.Count(r => r.Status == RegistrationStatus.Confirmed && r.CheckedInAt != null)
            });

        var query =
            from e in context.Events.AsNoTracking()
            where !e.IsDeleted && (ownerId == null || e.OwnerId == ownerId.Value)
            join c in counts on e.Id equals c.EventId into grouped
            from c in grouped.DefaultIfEmpty()
            orderby e.StartAt descending, e.Id
            select new EventStatisticsRecord(
                e.Id, e.OwnerId, e.Title, e.Type, e.Status, e.StartAt, e.EndAt, e.Capacity,
                c == null ? 0 : c.Confirmed,
                c == null ? 0 : c.Cancelled,
                c == null ? 0 : c.CheckedIn);

        return await query.ToListAsync(cancellationToken);
    }

    public void Add(Event item) => context.Events.Add(item);
}
