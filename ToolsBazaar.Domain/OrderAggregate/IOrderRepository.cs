namespace ToolsBazaar.Domain.OrderAggregate;

public interface IOrderRepository
{
    IEnumerable<Order> GetAll();

    IEnumerable<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
}