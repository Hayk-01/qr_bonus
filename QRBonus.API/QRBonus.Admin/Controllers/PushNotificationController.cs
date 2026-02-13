using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Constants;
using QRBonus.BLL.Services.PushNotificationService;
using QRBonus.DTO;

namespace QRBonus.Admin.Controllers;

public class PushNotificationController : ApiControllerBase
{
    private readonly IPushNotificationService _pushNotificationService;

    public PushNotificationController(IPushNotificationService pushNotificationService)
    {
        _pushNotificationService = pushNotificationService;
    }

    [HttpPost]
    public async Task<Result> SendNotification([FromBody] PushNotificationDto dto)
    {
        return await _pushNotificationService.SendToAllAsync(NotificationTopicConstants.AllUsers + dto.RegionId, dto.Title,
            dto.Message);
    }
}