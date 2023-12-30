using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;

namespace PTP.Domain.Entities;
public class Vehicle : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public byte NumberOfSeats { get; set; } = default!;
    public string LicensePlate { get; set; } = default!;
    public string Status { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Latitude { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Longitude { get; set; } = default!;
    public string VehicleType { get; set; } = nameof(VehicleTypeEnum.Bus);

    public ICollection<Trip> Trips { get; set; } = default!;
}