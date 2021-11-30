using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VeryBigShoeCompany.Common.Models;

namespace VeryBigShoeCompany.API.Models
{
    public class CsvResult : FileResult
    {
        private readonly IEnumerable<Order> _orders;
        public CsvResult(IEnumerable<Order> orders, string fileDownloadName) : base("text/csv")
        {
            _orders = orders;
            FileDownloadName = fileDownloadName;
        }
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });
            using (var streamWriter = new StreamWriter(response.Body))
            {
                var exportFileHeader = $"Customer Name, Customer Email, Quantity, Notes, Size, Date Required";
                await streamWriter.WriteLineAsync(exportFileHeader);
                foreach (var order in _orders)
                {
                    var exportFileLine = $"{order.CustomerName}, {order.CustomerEmail}, {order.Quantity}, {order.Notes}, {order.Size}, {order.DateToShow}";
                    await streamWriter.WriteLineAsync(exportFileLine);
                    await streamWriter.FlushAsync();
                }
                await streamWriter.FlushAsync();
            }
        }
    }
}
