using LetsWatch.Client.Dto;

namespace LetsWatch.Server.Services;

public interface ILetsWatchService
{
    public Task Update(WatchDto watchDto);
    public Task<WatchDto> Get(Guid userSession, long utcTicks);
}
