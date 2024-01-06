namespace PTP.Domain.Entities;
public class Trip : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string StartTime { get; set; } = default!;
    public string EndTime { get; set; } = default!;
    public string Status { get; set; } = string.Empty;


    #region Relationship Configuration
    public Guid TimeTableId { get; set; } = default!;
    public TimeTable TimeTable { get; set; } = default!;
    public ICollection<Schedule> Schedules { get; set; } = default!;
    #endregion


}