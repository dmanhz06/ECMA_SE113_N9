using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using EMCA.Application.DTOs;
using EMCA.Application.Services;
using Moq;
using Xunit;

namespace ECMA.Domain.Tests;

public sealed class EventStatisticsServiceTests
{
    private static readonly DateTime Now = new(2026, 11, 8, 2, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task AggregatesRegistrationCountsAndCompletedAttendance()
    {
        var ownerId = Guid.NewGuid();
        var rows = new[]
        {
            new EventStatisticsRecord(Guid.NewGuid(), ownerId, "Hội nghị tiếng Việt", EventType.Conference,
                EventStatus.Published, Now.AddDays(-2), Now, 50, 10, 3, 7),
            new EventStatisticsRecord(Guid.NewGuid(), ownerId, "Bản nháp quá giờ", EventType.Event,
                EventStatus.Draft, Now.AddDays(-2), Now.AddHours(-1), 20, 0, 0, 0),
            new EventStatisticsRecord(Guid.NewGuid(), ownerId, "Sắp diễn ra", EventType.Event,
                EventStatus.Published, Now.AddDays(1), Now.AddDays(2), 10, 4, 1, 2),
            new EventStatisticsRecord(Guid.NewGuid(), ownerId, "Đã hủy", EventType.Event,
                EventStatus.Cancelled, Now.AddDays(-1), Now.AddHours(-2), 5, 0, 2, 0)
        };
        var fixture = Create(UserRole.Organizer, ownerId, rows);
        var result = await fixture.Service.GetAsync(new(ownerId, UserRole.Organizer));

        Assert.True(result.IsSuccess);
        var stats = result.Value!;
        Assert.Equal(4, stats.TotalEvents);
        Assert.Equal(1, stats.DraftEvents);
        Assert.Equal(1, stats.PublishedEvents);
        Assert.Equal(1, stats.CompletedEvents);
        Assert.Equal(1, stats.CancelledEvents);
        Assert.Equal(20, stats.TotalRegistrations);
        Assert.Equal(14, stats.ConfirmedRegistrations);
        Assert.Equal(6, stats.CancelledRegistrations);
        Assert.Equal(9, stats.CheckedInRegistrations);
        var completed = Assert.Single(stats.Events, x => x.EventTitle == "Hội nghị tiếng Việt");
        Assert.Equal(3, completed.NotCheckedIn);
        Assert.Equal(70.0m, completed.AttendanceRate);
        Assert.Equal(3, completed.AbsentCount);
        var draft = Assert.Single(stats.Events, x => x.EventTitle == "Bản nháp quá giờ");
        Assert.Equal(EffectiveEventStatus.Draft, draft.EventStatus);
        Assert.Null(draft.AbsentCount);
        var published = Assert.Single(stats.Events, x => x.EventTitle == "Sắp diễn ra");
        Assert.Equal(50.0m, published.AttendanceRate);
        Assert.Null(published.AbsentCount);
    }

    [Fact]
    public async Task ZeroConfirmedRegistrationsProducesZeroRate()
    {
        var id = Guid.NewGuid();
        var rows = new[] { new EventStatisticsRecord(Guid.NewGuid(), id, "Sự kiện", EventType.Event,
            EventStatus.Published, Now.AddDays(1), Now.AddDays(2), 10, 0, 2, 0) };
        var fixture = Create(UserRole.Organizer, id, rows);
        var result = await fixture.Service.GetAsync(new(id, UserRole.Organizer));
        Assert.True(result.IsSuccess);
        Assert.Equal(0m, Assert.Single(result.Value!.Events).AttendanceRate);
    }

    [Fact]
    public async Task AttendeeIsForbidden()
    {
        var id = Guid.NewGuid();
        var fixture = Create(UserRole.Attendee, id, []);
        var result = await fixture.Service.GetAsync(new(id, UserRole.Attendee));
        Assert.False(result.IsSuccess);
        Assert.Equal("Forbidden", result.Error!.Code);
    }

    [Fact]
    public async Task InactiveUserIsForbidden()
    {
        var id = Guid.NewGuid();
        var fixture = Create(UserRole.Admin, id, [], UserStatus.Inactive);
        var result = await fixture.Service.GetAsync(new(id, UserRole.Admin));
        Assert.False(result.IsSuccess);
        Assert.Equal("Forbidden", result.Error!.Code);
    }

    [Fact]
    public async Task EmptyDatasetReturnsZeroesAndAdminIsNotOwnerScoped()
    {
        var id = Guid.NewGuid();
        var fixture = Create(UserRole.Admin, id, []);
        var result = await fixture.Service.GetAsync(new(id, UserRole.Admin));
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value!.TotalEvents);
        Assert.Equal(0, result.Value.TotalRegistrations);
        Assert.Empty(result.Value.Events);
        fixture.Events.Verify(x => x.GetStatisticsAsync(null, Now, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OrganizerStatisticsAreScopedToTheirOwnEvents()
    {
        var id = Guid.NewGuid();
        var fixture = Create(UserRole.Organizer, id, []);
        await fixture.Service.GetAsync(new(id, UserRole.Organizer));
        fixture.Events.Verify(x => x.GetStatisticsAsync(id, Now, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static (EventStatisticsService Service, Mock<IEventRepository> Events) Create(
        UserRole role, Guid userId, IReadOnlyList<EventStatisticsRecord> rows, UserStatus status = UserStatus.Active)
    {
        var users = new Mock<IUserRepository>();
        users.Setup(x => x.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User("tester", "hash", "stamp", "Người dùng thử", role, status));
        var events = new Mock<IEventRepository>();
        events.Setup(x => x.GetStatisticsAsync(It.IsAny<Guid?>(), Now, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid? ownerId, DateTime _, CancellationToken _) =>
                (IReadOnlyList<EventStatisticsRecord>)rows.Where(r => ownerId is null || r.OwnerId == ownerId).ToList());
        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Users).Returns(users.Object);
        uow.SetupGet(x => x.Events).Returns(events.Object);
        var clock = new Mock<IClock>();
        clock.SetupGet(x => x.UtcNow).Returns(Now);
        return (new EventStatisticsService(uow.Object, clock.Object), events);
    }
}
