using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using Xunit;

namespace ECMA.Domain.Tests;

public sealed class EventTests
{
    private static readonly DateTime Now = new(2026, 10, 19, 8, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Conference_PublishRequiresAgenda_ThenLocksAgenda()
    {
        var item = Valid(EventType.Conference);
        Assert.Equal("Conflict", item.Publish(Now).Error!.Code);
        Assert.True(item.AddAgenda("Khai mạc", null, Now.AddHours(2), Now.AddHours(2).AddMinutes(5)).IsSuccess);
        Assert.True(item.Publish(Now).IsSuccess);
        Assert.False(item.AddAgenda("Phiên sau", null, Now.AddHours(3), Now.AddHours(4)).IsSuccess);
    }

    [Fact]
    public void Agenda_RejectsOverlap_AllowsTouchingBoundary()
    {
        var item = Valid(EventType.Event);
        Assert.True(item.AddAgenda("Phiên một", null, Now.AddHours(2), Now.AddHours(3)).IsSuccess);
        Assert.False(item.AddAgenda("Chồng lấn", null, Now.AddHours(2).AddMinutes(30), Now.AddHours(3).AddMinutes(30)).IsSuccess);
        Assert.True(item.AddAgenda("Chạm biên", null, Now.AddHours(3), Now.AddHours(4)).IsSuccess);
    }

    [Fact]
    public void DraftTimeChange_RejectsAgendaOutsideNewRange()
    {
        var item = Valid(EventType.Event);
        Assert.True(item.AddAgenda("Phiên cuối", null, Now.AddHours(4), Now.AddHours(5)).IsSuccess);
        var result = item.UpdateDraft(EventType.Event, "Sự kiện mới", "Mô tả", "Hà Nội", Now.AddHours(1), Now.AddHours(4), 100, Now);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Published_UsesDerivedCompleted_AndOnlyCapacityIncrease()
    {
        var item = Valid(EventType.Event);
        Assert.True(item.Publish(Now).IsSuccess);
        Assert.False(item.UpdatePublished("Sự kiện mới", "Mô tả", "Hà Nội", 99, Now).IsSuccess);
        Assert.True(item.UpdatePublished("Sự kiện mới", "Mô tả", "Hà Nội", 101, Now).IsSuccess);
        Assert.Equal(EffectiveEventStatus.Completed, item.EffectiveStatus(Now.AddHours(7)));
    }

    [Fact]
    public void Values_AreNfcTrimmed_ButInvalidPrecisionIsRejected()
    {
        var valid = Event.Create(Guid.NewGuid(), EventType.Event, "  Sự kiện  ", " Mô tả ", " Hà Nội ",
            Now.AddHours(1), Now.AddHours(2), 10, Now);
        var seconds = Event.Create(Guid.NewGuid(), EventType.Event, "Sự kiện", "Mô tả", "Hà Nội",
            Now.AddHours(1).AddSeconds(1), Now.AddHours(2), 10, Now);
        var local = Event.Create(Guid.NewGuid(), EventType.Event, "Sự kiện", "Mô tả", "Hà Nội",
            DateTime.SpecifyKind(Now.AddHours(1), DateTimeKind.Local), Now.AddHours(2), 10, Now);

        Assert.Equal("Sự kiện", valid.Value!.Title);
        Assert.Equal("Time", seconds.Error!.Code);
        Assert.Equal("Time", local.Error!.Code);
    }

    [Fact]
    public void UnicodeLength_CountsScalars_NotGraphemeClusters()
    {
        var fourScalars = "👨‍👩";
        var result = Event.Create(Guid.NewGuid(), EventType.Event, fourScalars, "Mô tả", "Hà Nội",
            Now.AddHours(1), Now.AddHours(2), 10, Now);

        Assert.Equal("Title", result.Error!.Code);
    }

    [Fact]
    public void Agenda_SeparatesValidationFromOverlapConflict()
    {
        var item = Valid(EventType.Event);
        Assert.Equal("Agenda", item.AddAgenda("Ab", null, Now.AddHours(2), Now.AddHours(3)).Error!.Code);
        Assert.True(item.AddAgenda("Phiên một", null, Now.AddHours(2), Now.AddHours(3)).IsSuccess);
        Assert.Equal("Conflict", item.AddAgenda("Phiên hai", null, Now.AddHours(2), Now.AddHours(3)).Error!.Code);
    }

    private static Event Valid(EventType type) => Event.Create(Guid.NewGuid(), type, "Sự kiện mẫu", "Mô tả", "Hà Nội",
        Now.AddHours(1), Now.AddHours(6), 100, Now).Value!;
}
