using ECMA.Domain.Enums;

namespace EMCA.Application.DTOs;

public sealed record EventStatisticsItemDto(
    Guid EventId, string EventTitle, EventType EventType, EffectiveEventStatus EventStatus,
    DateTime StartAt, DateTime EndAt, int Capacity, int ConfirmedRegistrations,
    int CancelledRegistrations, int CheckedInRegistrations, int RemainingSlots,
    int NotCheckedIn, decimal AttendanceRate, int? AbsentCount);

public sealed record EventStatisticsDto(
    int TotalEvents, int DraftEvents, int PublishedEvents, int CompletedEvents, int CancelledEvents,
    int TotalRegistrations, int ConfirmedRegistrations, int CancelledRegistrations, int CheckedInRegistrations,
    IReadOnlyList<EventStatisticsItemDto> Events);
