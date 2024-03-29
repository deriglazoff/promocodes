﻿namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public interface IGivePromoCodeModel
    {
        public string ServiceInfo { get; set; }

        public string PartnerName { get; set; }

        public string PromoCode { get; set; }

        public string Preference { get; set; }
    }
}