namespace PTP.Domain.Entities
{
	public class TimeFrame : BaseEntity
	{
		public byte StartTime { get; set; } = default!;
		public byte EndTime { get; set; } = default!;

		public ICollection<Session> Sessions { get; set; } = default!;
	}
}
