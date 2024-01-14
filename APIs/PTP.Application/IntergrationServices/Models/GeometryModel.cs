namespace PTP.Application.IntergrationServices.Models;

public class GeometryModel{
     public List<Result>? Results { get; set; }
    public string? Status { get; set; }
}


public class Result
{
    public List<AddressComponent>? AddressComponents { get; set; }
    public string? FormattedAddress { get; set; }
    public Geometry? Geometry { get; set; }
    public string? PlaceId { get; set; }
    public string? Reference { get; set; }
    public PlusCode? PlusCode { get; set; }
    public Compound? Compound { get; set; }
    public List<string>? Types { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}

public class AddressComponent
{
    public string? LongName { get; set; }
    public string? ShortName { get; set; }
}

public class Geometry
{
    public Location? Location { get; set; }
    public object? Boundary { get; set; }
}

public class Location
{
    public decimal Lat { get; set; }
    public decimal Lng { get; set; }
}

public class PlusCode
{
    public string? CompoundCode { get; set; }
    public string? GlobalCode { get; set; }
}

public class Compound
{
    public string? District { get; set; }
    public string? Commune { get; set; }
    public string? Province { get; set; }
}
