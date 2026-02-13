using Ardalis.Result;

namespace QRBonus.BLL.Services.PushNotificationService;

public interface IPushNotificationService
{
    Task<Result> SendToAllAsync(string topicName, string title, string body);
    Task SubscribeUsersToTopic(List<string> tokens, string topicName);
}