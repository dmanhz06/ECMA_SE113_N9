using System.Text;

namespace ECMA.Domain.Entities;

public sealed class AgendaItem
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Content { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }

    private AgendaItem() { }

    internal AgendaItem(Guid eventId, string title, string? content, DateTime startAt, DateTime endAt)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        Update(title, content, startAt, endAt);
    }

    internal void Update(string title, string? content, DateTime startAt, DateTime endAt)
    {
        Title = Normalize(title);
        Content = string.IsNullOrWhiteSpace(content) ? null : Normalize(content);
        StartAt = startAt;
        EndAt = endAt;
    }

    internal static string Normalize(string value) => value.Normalize(NormalizationForm.FormC).Trim();
    internal static int Length(string value) => value.EnumerateRunes().Count();
    internal static bool IsUtcMinute(DateTime value) => value.Kind == DateTimeKind.Utc && value.Second == 0 && value.Millisecond == 0 && value.Ticks % TimeSpan.TicksPerMillisecond == 0;
}
