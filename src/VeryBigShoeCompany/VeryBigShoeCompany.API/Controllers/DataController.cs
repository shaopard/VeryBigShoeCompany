using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VeryBigShoeCompany.API.Models;
using VeryBigShoeCompany.Common.Models;
using VeryBigShoeCompany.ServiceContracts.Orders;

namespace VeryBigShoeCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public DataController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("Orders")]
        public async Task<ActionResult<List<Order>>> Get()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        [HttpGet]
        [Route("Orders/Print")]
        public async Task<IActionResult> Download()
        {
            var content = await _orderService.GetOrdersForDownload();
            var fileDownloadName = $"Orders-{DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss")}.csv";
            var contentBytes = Encoding.ASCII.GetBytes(content);

            return File(contentBytes, "application/octet-stream", fileDownloadName);
        }

    }
}
