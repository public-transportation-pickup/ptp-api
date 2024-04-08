using PTP.Domain.Entities.MongoDbs;

namespace PTP.Application.ViewModels.MongoDbs.Notifications;
public class NotificationUpdateModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsSeen { get; set; } = false;
    public string Content { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public NotificationSourceEnum Source { get; set; }
    public DateTime CreateDate { get; set; }
}