using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRBonus.Shared.Enums;

namespace QRBonus.Admin.Controllers;
[ApiController]
[Authorize(Roles = $"{nameof(UserRole.Admin)}")]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    public ApiControllerBase()
    {


    }
}
