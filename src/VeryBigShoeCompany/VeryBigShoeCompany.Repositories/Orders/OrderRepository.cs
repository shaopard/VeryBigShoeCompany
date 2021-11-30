using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryBigShoeCompany.Common.Models;
using VeryBigShoeCompany.RepositoryContracts.Orders;

namespace VeryBigShoeCompany.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private static List<Order> _orders = _orders ?? new List<Order>();

        public async Task<ICollection<Order>> AddOrders(ICollection<Order> orders)
        {
            await Task.Delay(0);

            _orders.AddRange(orders);

            return orders;
        }

        public async Task<ICollection<Order>> GetOrders()
        {
            await Task.Delay(0);
            return _orders;
        }
    }
}
