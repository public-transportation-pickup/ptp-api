namespace PTP.Domain.Entities
{
	public class Session : BaseEntity
	{
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;

		public DateTime StartedDate { get; set; } = DateTime.UtcNow;
		public DateTime? EndedDate { get; set; } = null;
		

		#region Relationship Configuration
		public Guid TimeFrameId { get; set; }
		public TimeFrame TimeFrame { get; set; } = default!;
		public Guid StoreId { get; set; } = default!;
		public Store Store { get; set; } = default!;
		#endregion

	}
}
