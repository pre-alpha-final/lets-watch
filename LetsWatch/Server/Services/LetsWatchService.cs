using LetsWatch.Client.Dto;
using LetsWatch.Server.Extensions;

namespace LetsWatch.Server.Services;

public class LetsWatchService : ILetsWatchService
{
    private static SemaphoreSlim _sync = new(1, 1);
    private static List<WatchDto> _watchDtos = new();
    private static TaskCompletionSource _updateTcs = new();

    public async Task Update(WatchDto watchDto)
    {
        using var sync = await _sync.DisposableWaitAsync(TimeSpan.MaxValue);

        var oldWatchDto = _watchDtos.FirstOrDefault(e => e.AdminSession == watchDto.AdminSession);
        if (oldWatchDto != null)
        {
            _watchDtos.Remove(oldWatchDto);
        }

        _watchDtos.Add(watchDto);

        _updateTcs.SetResult();
        _updateTcs = new();
    }

    public async Task<WatchDto> Get(Guid userSession, long utcTicks)
    {
        var updateTcs = _updateTcs;
        var watchDto = _watchDtos.FirstOrDefault(e => e.UserSession == userSession);
        if (watchDto.Timestamp.UtcTicks != utcTicks)
        {
            return Strip(watchDto);
        }

        await Task.WhenAny(updateTcs.Task, Task.Delay(TimeSpan.FromSeconds(30)));

        return Strip(_watchDtos.FirstOrDefault(e => e.UserSession == userSession));
    }

    private WatchDto Strip(WatchDto watchDto)
    {
        return new()
        {
            Title = watchDto.Title,
            Timestamp = watchDto.Timestamp,
        };
    }
}
