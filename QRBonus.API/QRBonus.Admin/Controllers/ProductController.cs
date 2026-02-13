using Ardalis.Result;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Filters;
using QRBonus.BLL.Services.ProductService;
using QRBonus.DTO;
using QRBonus.DTO.ProductDtos;

namespace QRBonus.Admin.Controllers;

public class ProductController: ApiControllerBase
{
    public readonly IProductAdminService _productService;

    public ProductController(IProductAdminService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<PagedResult<List<ProductDto>>> GetAll([FromQuery] ProductFilter filter)
    {
        return await _productService.GetAll(filter);
    }

    [HttpGet("{id}")]
    public async Task<Result<AddOrUpdateProductDto>> GetById(long id)
    {
        return await _productService.GetByIdAdmin(id);
    }

    [HttpPost]
    public async Task<Result<BaseDto>> Add([FromBody] AddOrUpdateProductDto dto)
    {
        return await _productService.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task<Result> Update(long id, [FromBody] AddOrUpdateProductDto dto)
    {
        return await _productService.Update(id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<Result> Delete(long id)
    {
        return await _productService.Delete(id);
    }
}