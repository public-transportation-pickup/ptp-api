namespace PTP.Application.ViewModels.RouteVars;
public class RouteVarCreateModel
{
    public int RouteVarId { get; set; }
    public string RouteVarName { get; set; } = string.Empty;
    public string RouteVarShortName { get; set; } = default!;
	public string StartStop { get; set; } = default!;
	public string EndStop { get; set; } = default!;
	public bool OutBound { get; set; } = true;
	public int RunningTime { get; set; } = 0;
	public double Distance { get; set; } = 0;
    public Guid RouteId { get; set; } = default!;
}