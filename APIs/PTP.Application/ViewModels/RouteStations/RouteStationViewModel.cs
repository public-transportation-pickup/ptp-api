namespace PTP.Application.ViewModels.RouteStations
{
	public class RouteStationViewModel
	{
		public Guid Id { get; set; }
		public int Index { get; set; } = 0;
		public double DistanceFromStart { get; set; } = default!;
		public double DistanceToNext { get; set; } = default!;
		public double DurationToNext { get; set; } = default!;
		public Guid StoreId { get; set;  } = Guid.Empty;
		public double DurationFromStart { get; set; } = default!;
		public decimal Latitude { get; set; } = default!;
		public decimal Longitude { get; set; } = default!;
		public Guid StationId { get; set; } = default!;
		public string StationName { get; set; } = default!;

		public static decimal[,] To(List<RouteStationViewModel> sources)
		{
			ArgumentNullException.ThrowIfNull(sources);
			var rows = sources.Count;
			const int cols = 2;
			decimal[,] result = new decimal[rows, cols];
			int count = 0;
			foreach (var item in sources)
			{
				result[count, 0] = item.Latitude;
				result[count, 1] = item.Longitude;
				count++;

			}
			return result;
		}
	}


}
