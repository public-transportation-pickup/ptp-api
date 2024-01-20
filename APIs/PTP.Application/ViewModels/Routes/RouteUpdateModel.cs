namespace PTP.Application.ViewModels.Routes;
public class RouteUpdateModel 
{
    public string Name { get; set; } = default!;

    public string RouteType { get; set; } = default!;
	public string TimeOfTrip { get; set; } = default!;
	public string HeadWay { get; set; } = default!;
	public string OperationTime { get; set; } = default!;
	public string OutBoundName { get; set; } = default!;
	public string NumOfSeats { get; set; } = default!;
	public string InBoundName { get; set; } = default!;
	public string OutBoundDescription { get; set; } = default!;
	public string InBoundDescription { get; set; } = string.Empty;
	public string TotalTrip { get; set; } = string.Empty;
	public string Orgs { get; set; } = default!;
	public string Tickets { get; set; } = default!;
}