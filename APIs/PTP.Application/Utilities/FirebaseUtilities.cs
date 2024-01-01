using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace PTP.Application.Utilities;
public static class FirebaseUtilities
{
    public static async Task<bool> SendNotification(string fcmToken, string title, string body, string senderId, string serverKey)
    {
        using var client = new HttpClient();
        // Replace with ServerId, SenderId, using Messaging Legacy API 
        // Deprecated on June-2024
        var firebaseOptionsServerId = serverKey;
        var firebaseOptionsSenderId = senderId;
        client.BaseAddress = new Uri("https://fcm.googleapis.com");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
            $"key={firebaseOptionsServerId}");
        client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={firebaseOptionsSenderId}");
        var data = new
        {
            to = fcmToken,
            notification = new
            {
                body,
                title,
            },
            priority = "high"
        };

        var json = JsonConvert.SerializeObject(data);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var result = await client.PostAsync("/fcm/send", httpContent);
        return result.StatusCode.Equals(HttpStatusCode.OK);
    }
}