using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System;
using Bogus;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.Data
{
    public static class PartnersBuilder
    {
        public static PartnerPromoCodeLimit PromoCodeLimit()
        {
            return new Faker<PartnerPromoCodeLimit>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.CreateDate, f => f.Date.Future())
                .RuleFor(u => u.EndDate, f => f.Date.Future())
                .RuleFor(u => u.Limit, f => f.UniqueIndex)
                .RuleFor(u=> u.CancelDate, f => f.Date.Future(-2))
                .Generate();
        }
        public static Partner CreateBasePartner()
        {
            return new Faker<Partner>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Name, f => f.Name.LastName())
                .RuleFor(u => u.IsActive, f => true)
                .RuleFor(u => u.PartnerLimits, f => new List<PartnerPromoCodeLimit>() { PromoCodeLimit() })
                .Generate();
        }

        public static Partner SetActiveLimit(this Partner partner)
        {
            partner.PartnerLimits = new List<PartnerPromoCodeLimit>
            {
                new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("A2998FA2-ED6A-44E8-A715-77F517B66B9A"),
                        CreateDate = new DateTime(2023, 02, 22),
                        EndDate = new DateTime(2023, 12, 9),
                        Limit = 100
                    }
            };

            return partner;
        }
    }
}
