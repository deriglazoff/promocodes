using System;

namespace Otus.Teaching.PromoCodeFactory.Domain.Models
{
    public class SetPartnerPromoCodeLimitRequest
    {
        public DateTime EndDate { get; set; }
        public int Limit { get; set; }
    }
}