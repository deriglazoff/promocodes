using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public interface IUnitOfWork
    {

        public IRepository<PromoCode> PromoCode { get; init; }

        public IRepository<Customer> Customer { get; init; }


        public void GivePromocodesToCustomersWithPreferenceAsync(IGivePromoCodeModel request);
    }
}
