using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml.Schema;
using VeryBigShoeCompany.Common.Exceptions;
using VeryBigShoeCompany.Common.Models;
using VeryBigShoeCompany.ServiceContracts.Orders;

namespace VeryBigShoeCompany.Services.Orders
{
    public class OrderValidatorService : IOrderValidatorService
    {
        public void Validate(ICollection<Order> orders)
        {
            foreach(var order in orders)
            {
                Validate(order);
            }
        }

        private static void Validate(Order order)
        {
            ValidateExistingCustomerName(order);
            ValidateEmail(order);
            ValidateDate(order);
            ValidateSize(order);
            ValidateQuantity(order);
        }

        private static void ValidateExistingCustomerName(Order order) 
        {
            if (string.IsNullOrWhiteSpace(order.CustomerName))
            {
                throw new BadRequestException("Is missing customer name.");
            }
        }

        private static void ValidateEmail(Order order)
        {
            if (!MailAddress.TryCreate(order.CustomerEmail, out MailAddress mailAddress))
            {
                throw new BadRequestException("Invalid email.");
            }
        } 

        private static void ValidateDate(Order order)
        {
            var currentDate = DateTime.Now.Date;
            DateTime firstValidDate;

            switch (currentDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    //if the start date is not a sunday you need to add 
                    //1 day to push it to a monday that is why the number is 15.
                    firstValidDate = currentDate.AddDays(15);
                    break;
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    //if the start date is any other day then just add 14 days to the start date.
                    firstValidDate = currentDate.AddDays(14);
                    break;
                case DayOfWeek.Saturday:
                    //if the start date is on a Saturday you need to add 
                    //2 days to push it to a monday that is why the number is 16.
                    firstValidDate = currentDate.AddDays(16);
                    break;
                default:
                    firstValidDate = currentDate;
                    break;
            }

            if (DateTime.Compare(order.DateRequired.Date, firstValidDate.Date) < 0)
            {
                throw new BadRequestException("Invalid Date. The date supplied is less than 10 working days into the future.");
            }
        }

        private static void ValidateSize(Order order)
        {
            if (order.Size < 11.5 || order.Size > 15.0)
            {
                throw new BadRequestException("Invalid Size.");
            }
        }

        private static void ValidateQuantity(Order order) 
        { 
            if (order.Quantity % 1000 > 0)
            {
                throw new BadRequestException("Invalid Quantity.");
            }
        }  
    }
}
