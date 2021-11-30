using System;

namespace VeryBigShoeCompany.Common.Models
{
    public class Order
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public double Size { get; set; }
        public DateTime DateRequired { get; set; }
        public string DateToShow => DateRequired.ToString("dd/MM/yyyy");
    }
}
