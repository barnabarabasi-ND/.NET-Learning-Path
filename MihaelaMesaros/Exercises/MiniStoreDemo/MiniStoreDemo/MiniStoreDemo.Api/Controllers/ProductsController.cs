using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniStoreDemo.Api.Common;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Application.Services;

namespace MiniStoreDemo.Api.Controllers;

/// <summary>
/// Controller responsible for handling product-related operations.
/// </summary>
/// <param name="productService">The service used to manage products.</param>
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of products based on the provided query parameters.
    /// </summary>
    /// <param name="queryParameters">Filter and pagination parameters for querying products.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of products matching the query parameters.</returns>
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

    /// <summary>
    /// Retrieves a specific product by its Id.
    /// </summary>
    /// <param name="id">The Id of product to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The product matching the specified Id.</returns>
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
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="productDto">The product data to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created product.</returns>
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

    /// <summary>
    /// Updates an existing product by its Id.
    /// </summary>
    /// <param name="id">The Id of product to update.</param>
    /// <param name="productDto">The updated product data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated product.</returns>
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

    /// <summary>
    /// Partially updates an existing product by its Id.
    /// </summary>
    /// <param name="id">The Id of the product to update.</param>
    /// <param name="productDto">The updated product data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated product.</returns>
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

    /// <summary>
    /// Deletes an existing product by its Id.
    /// </summary>
    /// <param name="id">The Id of the product to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if the deletion is successful.</returns>
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
