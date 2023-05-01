using Microsoft.AspNetCore.Mvc;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.ProductAggregate;

namespace ToolsBazaar.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase {

    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    [HttpGet("most-expensive")]
    public IActionResult GetMostExpensiveProducts()
    {
        var products = _productRepository.GetAll()
                            .OrderByDescending(p => p.Price)
                            .ThenBy(p => p.Name)
                            .ToList(); 

        return Ok(products);
    }




}