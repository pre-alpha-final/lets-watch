using LetsWatch.Client.Dto;
using LetsWatch.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsWatch.Server.Controllers;

[ApiController]
public class LetsWatchController : ControllerBase
{
    private readonly ILetsWatchService _letsWatchService;

    public LetsWatchController(ILetsWatchService letsWatchService)
    {
        _letsWatchService = letsWatchService;
    }

    [HttpPost("/api/session")]
    public async Task PostAsync(WatchDto watchDto)
    {
        await _letsWatchService.Update(watchDto);
    }

    [HttpGet("/api/session/{userSession:guid}/{utcTicks:long}")]
    public async Task<WatchDto> GetAsync(Guid userSession, long utcTicks)
    {

        return await _letsWatchService.Get(userSession, utcTicks);
    }
}
