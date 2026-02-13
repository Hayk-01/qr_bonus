using Ardalis.Result;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace QRBonus.BLL.Services.PushNotificationService;

public class PushNotificationService : IPushNotificationService
{
    public PushNotificationService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("qr-bonus-af21a-firebase-adminsdk-fbsvc-1afc04dc26.json") //TODO: get from appsettings
            });
        }
    }

    public async Task<Result> SendToAllAsync(string topicName, string title, string body)
    {
        var message = new Message()
        {
            Topic = topicName,
            Notification = new Notification() { Title = title, Body = body },
        };
        
        await FirebaseMessaging.DefaultInstance.SendAsync(message);

        return Result.Success();
    }

    public async Task SubscribeUsersToTopic(List<string> tokens, string topicName)
    {
        // You can send up to 1,000 tokens in a single batch
        var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
            tokens, topicName);

        Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully.");

        // Check for errors (e.g., invalid tokens)
        foreach (var error in response.Errors)
        {
            Console.WriteLine($"Token index {error.Index} failed: {error.Reason}");
        }
    }
}