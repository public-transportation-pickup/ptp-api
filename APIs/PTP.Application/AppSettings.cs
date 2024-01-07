namespace PTP.Application;
public class AppSettings
{
	public ConnectionStrings ConnectionStrings { get; set; } = default!;
	public FirebaseSettings FirebaseSettings { get; set; } = default!;
}

public class FirebaseSettings
{
	public string SenderId { get; set; } = default!;
	public string ServerKey { get; set; } = default!;
}
public class ConnectionStrings
{
	public string DefaultConnection { get; set; } = default!;
	public string RedisConnection { get; set; } = default!;
}