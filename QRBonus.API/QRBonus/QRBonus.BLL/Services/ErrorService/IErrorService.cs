using QRBonus.DTO;

namespace QRBonus.BLL.Services.ErrorService;
public interface IErrorService
{
    Task<ErrorModelDto> GetById(long id);
    Task<string> GetErrorName(long id);

}