namespace LetsWatch.Client.Dto;

public class WatchDto
{
    public Guid AdminSession { get; set; }
    public Guid UserSession { get; set; }
    public string Title { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
