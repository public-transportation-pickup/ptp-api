namespace PTP.Application.ViewModels.MongoDbs.Notifications;
public class NotificationCreateModel
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public int Source { get; set; } 
}