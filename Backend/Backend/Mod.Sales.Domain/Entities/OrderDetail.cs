using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Sales.Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtSale { get; set; }
        public decimal LineTotal { get; set; }
    }
}
