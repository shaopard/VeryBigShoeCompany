using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryBigShoeCompany.Common.Models;

namespace VeryBigShoeCompany.ServiceContracts.Orders
{
    public interface IOrderValidatorService
    {
        void Validate(ICollection<Order> orders);
    }
}
