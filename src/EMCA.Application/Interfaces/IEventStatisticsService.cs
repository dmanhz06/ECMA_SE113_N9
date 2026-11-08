using ECMA.Domain.Enums;
using EMCA.Application.DTOs;

namespace EMCA.Application.Interfaces;

public interface IEventStatisticsService
{
    Task<ECMA.Domain.Results.Result<EventStatisticsDto>> GetAsync(EventActor actor, CancellationToken cancellationToken = default);
}
