namespace PTP.Application;
public class AppSettings
{
	public ConnectionStrings ConnectionStrings { get; set; } = default!;
	public FirebaseSettings FirebaseSettings { get; set; } = default!;
	public string GoongAPIKey { get; set; } = default!;
	public JWTOptions JWTOptions { get; set; } = default!;
	public VnPay VnPay { get; set; } = new();
}
public class JWTOptions
{
	public string Secret { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
}

public class FirebaseSettings
{
	public string SenderId { get; set; } = default!;
	public string ServerKey { get; set; } = default!;
	public string ApiKeY { get; set; } = default!;
	public string Bucket { get; set; } = default!;
	public string AuthEmail { get; set; } = default!;
	public string AuthPassword { get; set; } = default!;
}
public class ConnectionStrings
{
	public string DefaultConnection { get; set; } = default!;
	public string MongoDbConnection { get; set; } = string.Empty;
	public string RedisConnection { get; set; } = default!;
}
public class VnPay
{
	public string Vnp_TmnCode { get; set; } = string.Empty;
	public string Vnp_HashSecret { get; set; } = string.Empty;
}