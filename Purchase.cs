using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice4
{
    public class Purchase : ISingleObject
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public Purchase(int purchaseId, int productId, decimal total, DateTime date)
        {
            PurchaseId = purchaseId;
            ProductId = productId;
            Total = total;
            Date = date;
        }
    }
}
