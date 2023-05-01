using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Persistence.ErrorManager;

namespace ToolsBazaar.Persistence;

public class CustomerRepository : ICustomerRepository
{
    public IEnumerable<Customer> GetAll() => DataSet.AllCustomers;

    public void UpdateCustomerName(int customerId, string name)
    {
        var customer = DataSet.AllCustomers.FirstOrDefault(c => c.Id == customerId);

        if(customer == null) throw new NotFoundException("Customer could not be found");

        customer.UpdateName(name);
    }

}