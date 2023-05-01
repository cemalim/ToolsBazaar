using ToolsBazaar.Domain.OrderAggregate;

namespace ToolsBazaar.Persistence;

public class OrderRepository : IOrderRepository
{
    public IEnumerable<Order> GetAll() => DataSet.AllOrders;

    public IEnumerable<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
    {
        return DataSet.AllOrders.Where(o => o.Date >= startDate && o.Date <= endDate).ToList();
    }
}