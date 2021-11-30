using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VeryBigShoeCompany.API.Filters;
using VeryBigShoeCompany.Common.Exceptions;
using VeryBigShoeCompany.Common.Models;
using VeryBigShoeCompany.ServiceContracts.Orders;

namespace VeryBigShoeCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    public class UploadController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private IWebHostEnvironment _hostingEnvironment;

        public UploadController(IWebHostEnvironment environment, IOrderService orderService)
        {
            _hostingEnvironment = environment;
            _orderService = orderService;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult<Order>> Upload(IFormFile uploadedFile)
        {
            string localFilePath = await CopyFile(uploadedFile);

            ICollection<Order> uploadedOrders = await _orderService.HandleOrderImport(localFilePath);

            return Ok(new { orders = uploadedOrders, count = uploadedOrders.Count });
        }


        private async Task<string> CopyFile(IFormFile uploadedFile)
        {
            string uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");

            bool exists = Directory.Exists(uploadsPath);
            if (!exists)
            {
                Directory.CreateDirectory(uploadsPath);
            }
            
            string filePath;

            if (uploadedFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(uploadedFile.FileName);

                if (!".xml".Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    throw new BadRequestException("Not accepted file type.");
                }

                string untrustedFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                string fileNameForUpdate = $"{untrustedFileName}-{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}{fileExtension}";

                filePath = Path.Combine(uploadsPath, fileNameForUpdate);
                Stream fileStream = new FileStream(filePath, FileMode.Create);

                try
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            } 
            else
            {
                throw new BadRequestException();
            }

            return filePath;
        }
    }
}
