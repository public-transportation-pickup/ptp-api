namespace PTP.Application.ViewModels.Stations;
public class StationViewModel
{
    public Guid Id { get; set; }
    public int StopId { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string StopType { get; set; } = default!;
    public string Name { get; set; } = default!;

    public string Status { get; set; } = default!;
    public string Zone { get; set; } = default!;
    public string Ward { get; set; } = default!;
    public string AddressNo { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string SupportDisability { get; set; } = default!;
    public Guid StoreId { get; set; } = Guid.Empty;

    #region Location 
    public string Address { get; set; } = default!;

    public decimal Latitude { get; set; } = default!;

    public decimal Longitude { get; set; } = default!;
    #endregion
}