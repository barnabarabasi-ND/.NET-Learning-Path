using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Common;
using MiniStoreDemo.DTOs;
using MiniStoreDemo.Services;

namespace MiniStoreDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService, ILogger<ProductsController> logger) : ControllerBase
{

    // GET /api/products
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsAsync([FromQuery] ProductQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var products = await productService.GetProductsAsync(
            queryParameters.PageNumber, 
            queryParameters.PageSize, 
            queryParameters.CategoryId, 
            queryParameters.IsActive, 
            queryParameters.Keyword, 
            cancellationToken
        );

        return Ok(products);
    }

    // GET /api/products/5
    [Authorize]
    [HttpGet("{id:int}", Name = "GetProductById")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductByIdAsync(id, cancellationToken);

        if (product is null)
        {
            //logger.LogInformation("Product #{ProductId} was not found.", id);

            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Policy = "ManageProducts")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> AddProductAsync(CreateProductDto productDto, CancellationToken cancellationToken)
    {
        var result = await productService.AddProductAsync(productDto, cancellationToken);

        if (!result.IsSuccess)
        {
            return ErrorHttpMapper.ToProblemResult(result.Error!);
        }

        var createdProduct = result.Value!;

        return CreatedAtRoute("GetProductById", new { id = createdProduct.ProductId }, createdProduct);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "ManageProducts")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> UpdateProductAsync(int id, UpdateProductDto productDto, CancellationToken cancellationToken)
    {
        var result = await productService.UpdateProductAsync(id, productDto, cancellationToken);

        if (!result.IsSuccess)
        {
            return ErrorHttpMapper.ToProblemResult(result.Error!);
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Policy = "ManageProducts")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> PatchProductAsync(int id, PatchProductDto productDto, CancellationToken cancellationToken)
    {
        var result = await productService.PatchProductAsync(id, productDto, cancellationToken);

        if (!result.IsSuccess)
            return ErrorHttpMapper.ToProblemResult(result.Error!);

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "ManageProducts")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        var result = await productService.DeleteProductAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return ErrorHttpMapper.ToProblemResult(result.Error!);
        }

        return NoContent();
    }
}
