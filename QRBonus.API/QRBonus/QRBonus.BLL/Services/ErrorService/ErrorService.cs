using Microsoft.EntityFrameworkCore;
using QRBonus.DAL;
using QRBonus.DTO;

namespace QRBonus.BLL.Services.ErrorService;
public class ErrorService : IErrorService
{
    private readonly AppDbContext _db;

    public ErrorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ErrorModelDto> GetById(long id)
    {
        var error = await _db.Errors.FirstOrDefaultAsync(x => x.Id == id);

        return new ErrorModelDto()
        {
            Code = error.Id,
            Description = error.Name
        };
    }

    public async Task<string> GetErrorName(long id)
    {
        var errorName = await _db.Errors.Where(x => x.Id == id)
            .Select(t => t.Name)
            .FirstOrDefaultAsync();

        if (errorName == null)
        {
            return "Error not found";
        }

        return errorName;

    }
}