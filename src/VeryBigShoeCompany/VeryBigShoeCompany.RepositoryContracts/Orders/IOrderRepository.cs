using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryBigShoeCompany.Common.Models;

namespace VeryBigShoeCompany.RepositoryContracts.Orders
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> AddOrders(ICollection<Order> orders);
        Task<ICollection<Order>> GetOrders();
    }
}
