using ECMA.Domain.Entities;
using ECMA.Domain.Enums;

namespace ECMA.Domain.Interfaces;

public sealed record EventQuery(Guid ActorId, UserRole Role, string? Keyword, EventType? Type, EffectiveEventStatus? Status, int Page = 1);
public sealed record PublicEventQuery(string? Keyword, EventType? Type, DateTime? StartFrom, DateTime? StartBefore, int Page);
public sealed record PublicEventRow(Event Event, int Confirmed);

public sealed record EventStatisticsRecord(
    Guid EventId, Guid OwnerId, string EventTitle, EventType EventType, EventStatus StoredStatus,
    DateTime StartAt, DateTime EndAt, int Capacity, int ConfirmedRegistrations,
    int CancelledRegistrations, int CheckedInRegistrations);

public interface IEventRepository
{
    Task<Event?> FindAsync(Guid id, bool forUpdate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> ListAsync(EventQuery query, DateTime now, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PublicEventRow>> SearchPublicAsync(PublicEventQuery query, DateTime now, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EventStatisticsRecord>> GetStatisticsAsync(Guid? ownerId, DateTime now, CancellationToken cancellationToken = default);
    void Add(Event item);
}
