using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Services;
using MiniStoreDemo.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MiniStoreDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService, ILogger<ProductsController> logger) : ControllerBase
{

    // GET /api/products
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting products.");
        var products = await productService.GetProductsAsync(cancellationToken);
        return Ok(products);
    }

    // GET /api/products/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductByIdAsync(id, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product #{ProductId} was not found.", id);

            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> AddProductAsync(CreateProductDto postProductDto, CancellationToken cancellationToken)
    {
        var createdProductId = await productService.AddProductAsync(postProductDto, cancellationToken);

        return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createdProductId }, createdProductId);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var isProductUpdated = await productService.UpdateProductAsync(updateProductDto, cancellationToken);

        if (!isProductUpdated)
        {
            logger.LogWarning("Product #{ProductId} has not been updated.", updateProductDto.ProductId);
            return NotFound();
        }

        return CreatedAtAction(nameof(GetProductByIdAsync), new { id = updateProductDto.ProductId }, updateProductDto.ProductId);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await productService.DeleteProductAsync(id, cancellationToken);

        if (!deleted)
        {
            logger.LogWarning("Product #{ProductId} has not been deleted.", id);
            return NotFound();
        }

        return NoContent();
    }
}
