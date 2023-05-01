using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Web.Controllers;

namespace ToolsBazaar.Tests;

public class  CustomerControllerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomersController _controller;


    public CustomerControllerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _controller = new CustomersController(new Mock<ILogger<CustomersController>>().Object, _customerRepositoryMock.Object, _orderRepositoryMock.Object);
    }

    [Fact]
    public void GetTopCustomers_ReturnsTopFiveCustomersByTotalSpending_WhenDataExists()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Customer 1" },
            new Customer { Id = 2, Name = "Customer 2" },
            new Customer { Id = 3, Name = "Customer 3" }
        };

        var orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                Customer   = new Customer{Id = 1 },
                Date       = new DateTime(2019, 1, 1),
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1,Quantity = 2 ,Product = new Product{ Id = 1, Price = 5}   },
                    new OrderItem { Id = 2, Quantity = 2, Product = new Product{ Id = 2, Price = 10} },
                    new OrderItem { Id = 3, Quantity = 3, Product = new Product{ Id = 3, Price = 15} }
                }
            },
           new Order
            {
                Id = 2,
                Customer   = new Customer{Id = 2 },
                Date       = new DateTime(2019, 1, 1),
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1,Quantity = 2 ,Product = new Product{ Id = 1, Price = 5}   },
                    new OrderItem { Id = 2, Quantity = 2, Product = new Product{ Id = 2, Price = 10} },
                    new OrderItem { Id = 3, Quantity = 3, Product = new Product{ Id = 3, Price = 15} }
                }
            },
            new Order
            {
                Id = 3,
                Customer   = new Customer{Id = 3 },
                Date       = new DateTime(2022, 1, 1),
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1,Quantity = 2 ,Product = new Product{ Id = 1, Price = 5}   },
                    new OrderItem { Id = 2, Quantity = 2, Product = new Product{ Id = 2, Price = 10} },
                    new OrderItem { Id = 3, Quantity = 3, Product = new Product{ Id = 3, Price = 15} }
                }
            }
        };
        _orderRepositoryMock.Setup(x => x.GetOrdersByDateRange(
            new DateTime(2015, 1, 1), new DateTime(2022, 12, 31))).Returns(orders);
        _customerRepositoryMock.Setup(x => x.GetAll()).Returns(customers);

        // Act
        var result = _controller.GetTopCustomers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var topCustomers = Assert.IsType<List<Customer>>(okResult.Value);
        Assert.Equal(3, topCustomers.Count);

    }

    }
