using Newtonsoft.Json;
using PTP.Application.IntergrationServices.Interfaces;

namespace PTP.Application.IntergrationServices;
public class LocationService : ILocationService
{
	private const string BASE_URL = "https://rsapi.goong.io/DistanceMatrix";
	private readonly AppSettings _appSettings;
	public LocationService(AppSettings appSettings)
	{
		_appSettings = appSettings;
	}


	public async Task<double> GetDistance(decimal orgLat, decimal orgLng, decimal destLat, decimal destLng, string travelMode = "car")
	{
		using HttpClient httpClient = new();

		using var response = await httpClient.GetAsync($"{BASE_URL}?origins={orgLat},{orgLng}&destinations={destLat},{destLng}&vehicle={travelMode}&api_key={_appSettings.GoongAPIKey}");
		response.EnsureSuccessStatusCode();
		var resultData = JsonConvert.DeserializeObject<RootObject>(await response.Content.ReadAsStringAsync())!;

		return resultData.Rows.ToList().First().Elements[0].Distance.Value;
	}

	public class RootObject
	{
		public Row[] Rows { get; set; } = default!;
	}

	public class Row
	{
		public Element[] Elements { get; set; } = default!;
	}

	public class Element
	{
		public Distance Distance { get; set; } = default!;
		public Duration Duration { get; set; } = default!;
		public string Status { get; set; } = default!;
	}

	public class Distance
	{
		public string Text { get; set; } = default!;
		public int Value { get; set; }
	}

	public class Duration
	{
		public string Text { get; set; } = default!;
		public int Value { get; set; }
	}

}