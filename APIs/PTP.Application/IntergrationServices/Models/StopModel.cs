namespace PTP.Application.IntergrationServices.Models;
public class StopModel
{
    public int StopId { get; set; } = default!;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string StopType { get; set; } = string.Empty;
    public string Zone { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string AddressNo { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string SupportDisability { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Lat { get; set; }
    public decimal Lng { get; set; }
    public string Search { get; set; } = default!;
    public string Routes { get; set; } = default!;
    public int Index { get; set; } = 0;
}

/* int StopId,
string Code,
int Index,
string Name,
string StopType,
string Zone,
string Ward,
string AddressNo,
string Street,
string SupportDisability,
string Status,
decimal Lng,
decimal Lat,
string Search,
string Routes*/