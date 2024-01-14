namespace PTP.Application.ViewModels.RouteVars;
public class RouteVarViewModel
{
    public Guid Id { get; set; } = default!;
    public int RouteVarId { get; set; } = default!;
    public string RouteVarName { get; set; } = default!;
    public string RouteVarShortName { get; set; } = default!;
    public string StartStop { get; set; } = default!;
    public string EndStop { get; set; } = default!;
    public bool OutBound { get; set; } = default!;
    public int RunningTime { get; set; } = default!;
    public double Distance { get; set; } = default!;

}