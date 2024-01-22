namespace PTP.Application.ViewModels.Routes;
public class RouteCreateModel
{
    public string RouteNo { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RouteType { get; set; } = string.Empty;
    public string TimeOfTrip { get; set; } = string.Empty;
	public string HeadWay { get; set; } = string.Empty;
	public string OperationTime { get; set; } = string.Empty;
	public string OutBoundName { get; set; } = string.Empty;
	public string NumOfSeats { get; set; } = string.Empty;
	public string InBoundName { get; set; } = string.Empty;
	public string OutBoundDescription { get; set; } = string.Empty;
	public string InBoundDescription { get; set; } = string.Empty;
	public string TotalTrip { get; set; } = string.Empty;
	public string Orgs { get; set; } = string.Empty;
	public string Tickets { get; set; } = string.Empty;
}