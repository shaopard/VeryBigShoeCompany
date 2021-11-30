using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryBigShoeCompany.Common.Models;

namespace VeryBigShoeCompany.ServiceContracts.Orders
{
    public interface IOrderService
    {
        Task<ICollection<Order>> HandleOrderImport(string filePath);
        Task<ICollection<Order>> GetOrders();
        Task<string> GetOrdersForDownload();
    }
}
