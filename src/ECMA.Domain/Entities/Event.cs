using ECMA.Domain.Enums;
using ECMA.Domain.Results;

namespace ECMA.Domain.Entities;

public sealed class Event
{
    private readonly List<AgendaItem> _agendaItems = [];

    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public EventType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public int Capacity { get; private set; }
    public EventStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public Guid? CancelledBy { get; private set; }
    public string? CancelReason { get; private set; }
    public uint Version { get; private set; }
    public IReadOnlyCollection<AgendaItem> AgendaItems => _agendaItems;

    private Event() { }

    private Event(Guid ownerId, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        CreatedAt = createdAt;
        Status = EventStatus.Draft;
    }

    public static Result<Event> Create(Guid ownerId, EventType type, string title, string description, string location,
        DateTime startAt, DateTime endAt, int capacity, DateTime now)
    {
        var item = new Event(ownerId, now);
        var update = item.SetDraftData(type, title, description, location, startAt, endAt, capacity, now);
        return update.IsFailure ? Result<Event>.Failure(update.Error!) : Result<Event>.Success(item);
    }

    public EffectiveEventStatus EffectiveStatus(DateTime now) => Status == EventStatus.Published && now >= EndAt
        ? EffectiveEventStatus.Completed : (EffectiveEventStatus)Status;

    public Result UpdateDraft(EventType type, string title, string description, string location,
        DateTime startAt, DateTime endAt, int capacity, DateTime now)
    {
        if (Status != EventStatus.Draft || IsDeleted) return Conflict("Sự kiện không ở trạng thái bản nháp");
        return SetDraftData(type, title, description, location, startAt, endAt, capacity, now);
    }

    public Result UpdatePublished(string title, string description, string location, int capacity, DateTime now)
    {
        if (Status != EventStatus.Published || now >= StartAt) return Conflict("Sự kiện không còn cho phép chỉnh sửa");
        var normalizedTitle = AgendaItem.Normalize(title);
        var normalizedDescription = AgendaItem.Normalize(description);
        var normalizedLocation = AgendaItem.Normalize(location);
        var common = ValidateText(normalizedTitle, normalizedDescription, normalizedLocation, capacity);
        if (common.IsFailure) return common;
        if (capacity < Capacity) return Conflict("Sức chứa chỉ được giữ nguyên hoặc tăng");
        Title = normalizedTitle; Description = normalizedDescription; Location = normalizedLocation; Capacity = capacity;
        return Result.Success();
    }

    public Result<AgendaItem> AddAgenda(string title, string? content, DateTime startAt, DateTime endAt)
    {
        if (Status != EventStatus.Draft || IsDeleted) return Result<AgendaItem>.Failure(Error.Conflict("Chương trình đã bị khóa"));
        if (_agendaItems.Count >= 50) return Result<AgendaItem>.Failure(Error.Conflict("Chương trình chỉ được tối đa 50 mục"));
        var item = new AgendaItem(Id, title, content, startAt, endAt);
        var validation = ValidateAgenda(item, null);
        if (validation.IsFailure) return Result<AgendaItem>.Failure(validation.Error!);
        _agendaItems.Add(item);
        return Result<AgendaItem>.Success(item);
    }

    public Result UpdateAgenda(Guid itemId, string title, string? content, DateTime startAt, DateTime endAt)
    {
        if (Status != EventStatus.Draft || IsDeleted) return Conflict("Chương trình đã bị khóa");
        var item = _agendaItems.SingleOrDefault(x => x.Id == itemId);
        if (item is null) return Result.Failure(Error.NotFound("Không tìm thấy dữ liệu"));
        var candidate = new AgendaItem(Id, title, content, startAt, endAt);
        var validation = ValidateAgenda(candidate, itemId);
        if (validation.IsFailure) return validation;
        item.Update(title, content, startAt, endAt);
        return Result.Success();
    }

    public Result DeleteAgenda(Guid itemId)
    {
        if (Status != EventStatus.Draft || IsDeleted) return Conflict("Chương trình đã bị khóa");
        var item = _agendaItems.SingleOrDefault(x => x.Id == itemId);
        if (item is null) return Result.Failure(Error.NotFound("Không tìm thấy dữ liệu"));
        _agendaItems.Remove(item);
        return Result.Success();
    }

    public Result Publish(DateTime now)
    {
        if (Status != EventStatus.Draft) return Conflict("Sự kiện không ở trạng thái bản nháp");
        if (now >= StartAt) return Conflict("Thời gian bắt đầu phải ở tương lai. Cập nhật thời gian trước khi công bố");
        if (Type == EventType.Conference && _agendaItems.Count == 0)
            return Conflict("Hội nghị cần ít nhất một mục chương trình để công bố");
        if (!IsAllDataValid()) return Conflict("Dữ liệu sự kiện hoặc chương trình không hợp lệ. Vui lòng kiểm tra lại trước khi công bố");
        Status = EventStatus.Published;
        return Result.Success();
    }

    public Result Cancel(string reason, Guid actorId, DateTime now)
    {
        if (Status != EventStatus.Published || now >= StartAt) return Conflict("Không thể hủy sự kiện đã bắt đầu");
        reason = AgendaItem.Normalize(reason);
        if (AgendaItem.Length(reason) is < 1 or > 200) return Result.Failure(Error.Validation("CancelReason", "Lý do hủy phải có từ 1 đến 200 ký tự"));
        Status = EventStatus.Cancelled; CancelledAt = now; CancelledBy = actorId; CancelReason = reason;
        return Result.Success();
    }

    public Result Delete()
    {
        if (IsDeleted) return Result.Failure(Error.NotFound("Không tìm thấy dữ liệu"));
        if (Status != EventStatus.Draft) return Conflict("Chỉ được xóa sự kiện bản nháp");
        IsDeleted = true;
        return Result.Success();
    }

    private Result SetDraftData(EventType type, string title, string description, string location,
        DateTime startAt, DateTime endAt, int capacity, DateTime now)
    {
        title = AgendaItem.Normalize(title); description = AgendaItem.Normalize(description); location = AgendaItem.Normalize(location);
        var common = ValidateText(title, description, location, capacity);
        if (common.IsFailure) return common;
        if (!Enum.IsDefined(type) || !AgendaItem.IsUtcMinute(startAt) || !AgendaItem.IsUtcMinute(endAt) || now.Kind != DateTimeKind.Utc || startAt <= now || endAt <= startAt || endAt - startAt < TimeSpan.FromMinutes(5) || endAt - startAt > TimeSpan.FromDays(30))
            return Result.Failure(Error.Validation("Time", "Thời gian sự kiện không hợp lệ"));
        if (_agendaItems.Any(x => x.StartAt < startAt || x.EndAt > endAt)) return Conflict("Chương trình nằm ngoài thời gian sự kiện");
        Type = type; Title = title; Description = description; Location = location; StartAt = startAt; EndAt = endAt; Capacity = capacity;
        return Result.Success();
    }

    private static Result ValidateText(string title, string description, string location, int capacity)
    {
        if (AgendaItem.Length(title) is < 5 or > 100) return Result.Failure(Error.Validation("Title", "Tiêu đề phải có từ 5 đến 100 ký tự"));
        if (AgendaItem.Length(description) is < 1 or > 3000) return Result.Failure(Error.Validation("Description", "Mô tả phải có từ 1 đến 3000 ký tự"));
        if (AgendaItem.Length(location) is < 1 or > 200) return Result.Failure(Error.Validation("Location", "Địa điểm phải có từ 1 đến 200 ký tự"));
        return capacity is < 1 or > 10000 ? Result.Failure(Error.Validation("Capacity", "Sức chứa phải từ 1 đến 10000")) : Result.Success();
    }

    private Result ValidateAgenda(AgendaItem item, Guid? ignoredId)
    {
        if (AgendaItem.Length(item.Title) is < 3 or > 100 || item.Content is not null && AgendaItem.Length(item.Content) > 1000 ||
            !AgendaItem.IsUtcMinute(item.StartAt) || !AgendaItem.IsUtcMinute(item.EndAt) || item.StartAt < StartAt || item.EndAt > EndAt ||
            item.EndAt - item.StartAt < TimeSpan.FromMinutes(5))
            return Result.Failure(Error.Validation("Agenda", "Mục chương trình không hợp lệ"));
        return _agendaItems.Any(x => x.Id != ignoredId && item.StartAt < x.EndAt && x.StartAt < item.EndAt)
            ? Conflict("Mục chương trình bị chồng lấn") : Result.Success();
    }

    private bool IsAllDataValid() => ValidateText(Title, Description, Location, Capacity).IsSuccess &&
        AgendaItem.IsUtcMinute(StartAt) && AgendaItem.IsUtcMinute(EndAt) && EndAt > StartAt && EndAt - StartAt >= TimeSpan.FromMinutes(5) && EndAt - StartAt <= TimeSpan.FromDays(30) &&
        _agendaItems.Count <= 50 && _agendaItems.All(x => ValidateAgenda(x, x.Id).IsSuccess);

    private static Result Conflict(string message) => Result.Failure(Error.Conflict(message));
}
