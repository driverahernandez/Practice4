using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice4
{
    public class Sale : ISingleObject
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Total {  get; set; }
        public DateTime Date { get; set; }

        public Sale(int orderId, int productId, decimal total, DateTime date)
        {
            OrderId = orderId;
            ProductId = productId;
            Total = total;
            Date = date;
        }
    }
}
