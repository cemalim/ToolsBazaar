using Microsoft.AspNetCore.Mvc;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Persistence.ErrorManager;

namespace ToolsBazaar.Web.Controllers;

public record CustomerDto(string Name);

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomersController> _logger;
    private readonly IOrderRepository _orderRepository;

    public CustomersController(ILogger<CustomersController> logger, ICustomerRepository customerRepository, IOrderRepository orderRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _orderRepository   = orderRepository;
    }

    [HttpPut("{customerId:int}")]
    public IActionResult UpdateCustomerName(int customerId, [FromRoute] CustomerDto dto)
    {
        try
        {
            _logger.LogInformation($"Updating customer #{customerId} name...");

            _customerRepository.UpdateCustomerName(customerId, dto.Name);

            return Ok();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex.ToString());

            return NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500, "General Error");
        }
    }



    [HttpGet("top")]
    public IActionResult GetTopCustomers()
    {
        var startDate = new DateTime(2015, 1, 1);
        var endDate = new DateTime(2022, 12, 31);

        var orders = _orderRepository.GetOrdersByDateRange(startDate, endDate);

        var query = from order in orders
                    join customer in _customerRepository.GetAll()
                        on order.Customer.Id equals customer.Id
                    select new
                    {
                        Order = order,
                        Customer = customer
                    };

        var customerOrderTotals = query
            .GroupBy(x => x.Customer)
            .Select(group => new
            {
                Customer = group.Key,
                Total = group.Sum(x => x.Order.Items
                    .Sum(item => item.Price * item.Quantity))
            })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .ToList();

        var topCustomers = customerOrderTotals
            .Select(x => new Customer
            {
                Name = x.Customer.Name,
                //TotalOrders = x.Customer.Orders.Count(),
                //TotalAmount = x.Total
            })
            .ToList();         

        return Ok(topCustomers);
    }


}