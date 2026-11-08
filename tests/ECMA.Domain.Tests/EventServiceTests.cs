using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using EMCA.Application.DTOs;
using EMCA.Application.Services;
using Moq;
using Xunit;

namespace ECMA.Domain.Tests;

public sealed class EventServiceTests
{
    [Fact]
    public async Task Organizer_CannotEditAnotherOwnersEvent()
    {
        var now = new DateTime(2026, 10, 19, 8, 0, 0, DateTimeKind.Utc);
        var item = Event.Create(Guid.NewGuid(), EventType.Event, "Sự kiện mẫu", "Mô tả", "Hà Nội", now.AddHours(1), now.AddHours(2), 10, now).Value!;
        var unit = new Mock<IUnitOfWork>();
        var transaction = new Mock<IUnitOfWorkTransaction>();
        var actor = new User("actor", "hash", "stamp", "Actor", UserRole.Organizer, UserStatus.Active);
        unit.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(transaction.Object);
        unit.Setup(x => x.Users.FindByIdForUpdateAsync(actor.Id, It.IsAny<CancellationToken>())).ReturnsAsync(actor);
        unit.Setup(x => x.Events.FindAsync(item.Id, true, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        var clock = new Mock<IClock>();
        clock.SetupGet(x => x.UtcNow).Returns(now);

        var result = await new EventService(unit.Object, clock.Object).EditDraftAsync(
            new EventActor(actor.Id, UserRole.Admin), item.Id,
            new EventData(EventType.Event, "Sự kiện khác", "Mô tả", "Đà Nẵng", now.AddHours(1), now.AddHours(2), 10));

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.Error!.Code);
        unit.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task PublishedOtherOwnersEvent_UsesAuthoritativeRoleForManagementCapability()
    {
        var now = new DateTime(2026, 10, 19, 8, 0, 0, DateTimeKind.Utc);
        var actor = new User("actor", "hash", "stamp", "Actor", UserRole.Organizer, UserStatus.Active);
        var item = Event.Create(Guid.NewGuid(), EventType.Event, "Sự kiện mẫu", "Mô tả", "Hà Nội", now.AddHours(1), now.AddHours(2), 10, now).Value!;
        Assert.True(item.Publish(now).IsSuccess);
        var unit = new Mock<IUnitOfWork>();
        unit.Setup(x => x.Users.FindByIdAsync(actor.Id, It.IsAny<CancellationToken>())).ReturnsAsync(actor);
        unit.Setup(x => x.Events.FindAsync(item.Id, false, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        var clock = new Mock<IClock>();
        clock.SetupGet(x => x.UtcNow).Returns(now);

        var result = await new EventService(unit.Object, clock.Object).GetAsync(new EventActor(actor.Id, UserRole.Admin), item.Id);

        Assert.True(result.IsSuccess);
        Assert.False(result.Value!.CanManage);
    }

    [Fact]
    public async Task InvalidFilter_IsRejectedBeforeRepositoryQuery()
    {
        var unit = new Mock<IUnitOfWork>();
        var clock = new Mock<IClock>();
        var result = await new EventService(unit.Object, clock.Object).ListAsync(
            new EventActor(Guid.NewGuid(), UserRole.Admin), new EventListRequest(Status: "1"));

        Assert.False(result.IsSuccess);
        Assert.Equal("BadRequest", result.Error!.Code);
        unit.Verify(x => x.Events.ListAsync(It.IsAny<EventQuery>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task InvalidPage_HasDistinctMessage()
    {
        var result = await new EventService(new Mock<IUnitOfWork>().Object, new Mock<IClock>().Object).ListAsync(
            new EventActor(Guid.NewGuid(), UserRole.Admin), new EventListRequest(Page: 0));

        Assert.Equal("Tham số trang không hợp lệ", result.Error!.Message);
    }
}
