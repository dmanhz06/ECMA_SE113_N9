using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using ECMA.Domain.Results;
using EMCA.Application.DTOs;
using EMCA.Application.Interfaces;

namespace EMCA.Application.Services;

public sealed class EventStatisticsService(IUnitOfWork unitOfWork, IClock clock) : IEventStatisticsService
{
    public async Task<Result<EventStatisticsDto>> GetAsync(EventActor actor, CancellationToken cancellationToken = default)
    {
        if (actor.UserId == Guid.Empty)
            return Result<EventStatisticsDto>.Failure(Error.Forbidden("Bạn không có quyền xem thống kê"));

        var user = await unitOfWork.Users.FindByIdAsync(actor.UserId, cancellationToken);
        if (user is null || user.Status != UserStatus.Active)
            return Result<EventStatisticsDto>.Failure(Error.Forbidden("Tài khoản không hoạt động hoặc không có quyền truy cập"));
        if (user.Role is not (UserRole.Admin or UserRole.Organizer) || user.Role != actor.Role)
            return Result<EventStatisticsDto>.Failure(Error.Forbidden("Bạn không có quyền xem thống kê"));

        var now = clock.UtcNow;
        var rows = await unitOfWork.Events.GetStatisticsAsync(
            user.Role == UserRole.Admin ? null : user.Id, now, cancellationToken);

        var items = rows.Select(row =>
        {
            var status = row.StoredStatus == EventStatus.Published && row.EndAt <= now
                ? EffectiveEventStatus.Completed
                : (EffectiveEventStatus)row.StoredStatus;
            var notCheckedIn = row.ConfirmedRegistrations - row.CheckedInRegistrations;
            var rate = row.ConfirmedRegistrations == 0
                ? 0m
                : Math.Round((decimal)row.CheckedInRegistrations / row.ConfirmedRegistrations * 100m, 1, MidpointRounding.AwayFromZero);
            return new EventStatisticsItemDto(
                row.EventId, row.EventTitle, row.EventType, status, row.StartAt, row.EndAt, row.Capacity,
                row.ConfirmedRegistrations, row.CancelledRegistrations, row.CheckedInRegistrations,
                row.Capacity - row.ConfirmedRegistrations, notCheckedIn, rate,
                status == EffectiveEventStatus.Completed ? notCheckedIn : null);
        }).ToList();

        return Result<EventStatisticsDto>.Success(new EventStatisticsDto(
            items.Count,
            items.Count(x => x.EventStatus == EffectiveEventStatus.Draft),
            items.Count(x => x.EventStatus == EffectiveEventStatus.Published),
            items.Count(x => x.EventStatus == EffectiveEventStatus.Completed),
            items.Count(x => x.EventStatus == EffectiveEventStatus.Cancelled),
            items.Sum(x => x.ConfirmedRegistrations + x.CancelledRegistrations),
            items.Sum(x => x.ConfirmedRegistrations),
            items.Sum(x => x.CancelledRegistrations),
            items.Sum(x => x.CheckedInRegistrations),
            items));
    }
}
