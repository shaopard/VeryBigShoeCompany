using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using VeryBigShoeCompany.Common.Exceptions;
using VeryBigShoeCompany.Common.Models;
using VeryBigShoeCompany.RepositoryContracts.Orders;
using VeryBigShoeCompany.ServiceContracts.Orders;

namespace VeryBigShoeCompany.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly string xsdPath;
        private readonly IOrderValidatorService _orderValidatorService;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderValidatorService orderValidatorService, IOrderRepository orderRepository)
        {
            xsdPath = Path.Combine(Environment.CurrentDirectory, @"XSD\OrderImport.xsd");

            _orderValidatorService = orderValidatorService;
            _orderRepository = orderRepository;
        }

        public async Task<ICollection<Order>> HandleOrderImport(string filePath)
        {
            var orders = ParseOrders(filePath);

            _orderValidatorService.Validate(orders);
            var ordersToReturn = await _orderRepository.AddOrders(orders);
            
            return ordersToReturn;
        }

        public async Task<string> GetOrdersForDownload()
        {
            var orders = await _orderRepository.GetOrders();

            var sb = new StringBuilder();
            sb.Append($"Customer Name, Customer Email, Quantity, Notes, Size, Date Required{Environment.NewLine}");

            foreach (var order in orders)
            {
                var exportFileLine = $"{order.CustomerName}, {order.CustomerEmail}, {order.Quantity}, {order.Notes}, {order.Size}, {order.DateToShow}{Environment.NewLine}";
                sb.Append(exportFileLine);
            }

            var content = sb.ToString();
            return content;
        }

        public async Task<ICollection<Order>> GetOrders()
        {
            var orders = await _orderRepository.GetOrders();
            return orders;
        }

        private async Task AddOrders(ICollection<Order> orders)
        {
            await _orderRepository.AddOrders(orders);
        }

        private ICollection<Order> ParseOrders(string filePath)
        {
            ValidateAgainstXsd(filePath);

            XElement bigShoeDataImport = XElement.Load(filePath);

            var orders = (from item in bigShoeDataImport.Descendants("Order")
                            select new Order
                            {
                                CustomerName = (string) item.Attribute("CustomerName"),
                                CustomerEmail = (string) item.Attribute("CustomerEmail"),
                                Quantity = (int) item.Attribute("Quantity"),
                                Notes = (string) item.Attribute("Notes"),
                                Size = (double) item.Attribute("Size"),
                                DateRequired = ParseDateRequired(item.Attribute("DateRequired")),
                            })
                            .ToList();
                                         
            return orders;
        }

        private static DateTime ParseDateRequired(XAttribute dateRequiredAtribute) => DateTime.Parse((string)dateRequiredAtribute);

        private void ValidateAgainstXsd(string filePath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(string.Empty, xsdPath);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationEventHandler);

            XmlReader reader = XmlReader.Create(filePath, settings);

            while (reader.Read()) { };
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new BadRequestException("The uploaded XML file fails schema validation.");
        }
    }
}
