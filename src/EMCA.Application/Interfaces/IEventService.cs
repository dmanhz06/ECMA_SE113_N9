using ECMA.Domain.Results;
using EMCA.Application.DTOs;

namespace EMCA.Application.Interfaces;

public interface IEventService
{
    Task<Result<IReadOnlyList<EventDto>>> ListAsync(EventActor actor, EventListRequest request, CancellationToken cancellationToken = default);
    Task<Result<EventDto>> GetAsync(EventActor actor, Guid id, CancellationToken cancellationToken = default);
    Task<Result<EventDto>> CreateAsync(EventActor actor, EventData data, CancellationToken cancellationToken = default);
    Task<Result<EventDto>> EditDraftAsync(EventActor actor, Guid id, EventData data, CancellationToken cancellationToken = default);
    Task<Result<EventDto>> EditPublishedAsync(EventActor actor, Guid id, PublishedEventData data, CancellationToken cancellationToken = default);
    Task<Result> PublishAsync(EventActor actor, Guid id, CancellationToken cancellationToken = default);
    Task<Result> CancelAsync(EventActor actor, Guid id, string reason, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(EventActor actor, Guid id, CancellationToken cancellationToken = default);
    Task<Result<AgendaDto>> AddAgendaAsync(EventActor actor, Guid eventId, AgendaData data, CancellationToken cancellationToken = default);
    Task<Result> EditAgendaAsync(EventActor actor, Guid eventId, Guid itemId, AgendaData data, CancellationToken cancellationToken = default);
    Task<Result> DeleteAgendaAsync(EventActor actor, Guid eventId, Guid itemId, CancellationToken cancellationToken = default);
}
