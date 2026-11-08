using ECMA.Domain.Enums;

namespace EMCA.Application.DTOs;

public sealed record EventActor(Guid UserId, UserRole Role);
public sealed record EventData(EventType Type, string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, int Capacity);
public sealed record PublishedEventData(string Title, string Description, string Location, int Capacity);
public sealed record AgendaData(string Title, string? Content, DateTime StartAt, DateTime EndAt);
public sealed record EventListRequest(string? Keyword = null, string? Type = null, string? Status = null, int Page = 1);
public sealed record AgendaDto(Guid Id, string Title, string? Content, DateTime StartAt, DateTime EndAt);
public sealed record EventDto(Guid Id, Guid OwnerId, EventType Type, string Title, string Description, string Location,
    DateTime StartAt, DateTime EndAt, int Capacity, EffectiveEventStatus Status, DateTime CreatedAt,
    string? CancelReason, IReadOnlyList<AgendaDto> Agenda, bool CanManage);
