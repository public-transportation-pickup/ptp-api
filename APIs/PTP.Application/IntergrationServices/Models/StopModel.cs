namespace PTP.Application.IntergrationServices.Models;
public record StopModel(
int StopId,
string Code,
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
string Routes
);