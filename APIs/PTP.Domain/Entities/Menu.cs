﻿using PTP.Domain.Enums;

namespace PTP.Domain.Entities;

public class Menu : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public TimeSpan StartTime { get; set; } = default!;
	public TimeSpan EndTime { get; set; } = default!;
	public DateTime DateApply { get; set; } = default!;
	public int NumOrderEstimated { get; set; }
	public int NumOrderSold { get; set; } = 0;
	public string Status { get; set; } = nameof(DefaultStatusEnum.Active);

	#region Relationship Configuration
	public Guid StoreId { get; set; }
	public Store Store { get; set; } = default!;
	public ICollection<ProductInMenu>? ProductInMenus { get; set; } = default!;

	#endregion
}

