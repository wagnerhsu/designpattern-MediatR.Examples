using Common.Event;
using System.Collections.Generic;

namespace Domain.Catalog.Event
{
    public class ProductsPurchased : DomainEvent
    {
        public int UserId { get; set; }

        public decimal UserDiscount { get; set; }

        public IEnumerable<PurchasedProduct> PurchasedProducts { get; set; }
    }
}