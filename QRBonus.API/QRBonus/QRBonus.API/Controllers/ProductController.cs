using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using QRBonus.BLL.Services.ProductService;
using QRBonus.DTO.ProductDtos;

namespace QRBonus.API.Controllers;

public class ProductController : ApiControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }


    [HttpGet("{id}")]
    public async Task<Result<ProductDto>> GetById(long id)
    {
        return await _productService.GetById(id);
    }
}
