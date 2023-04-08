using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public IRepository<PromoCode> PromoCode { get; init; }

        public IRepository<Customer> Customer { get; init; }

        private DataContext _dataContext;
        public UnitOfWork(IRepository<Customer> repositoryCustomer, IRepository<PromoCode> repositoryPromoCode, DataContext dataContext)
        {
            Customer = repositoryCustomer;
            PromoCode = repositoryPromoCode;
            _dataContext = dataContext;
        }

        public async void GivePromocodesToCustomersWithPreferenceAsync(IGivePromoCodeModel request)
        {
            var customers = _dataContext.Set<Customer>().Include(c => c.Preferences).Where(x => x.Preferences.Any(p => p.Name == request.Preference)).ToList();

            if (!customers.Any()) {
                throw new Exception("Клиентов с таким предпочтением нет");
            }
            var promocode = new PromoCode
            {
                ServiceInfo = request.ServiceInfo,
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
            };
            await _dataContext.Set<PromoCode>().AddAsync(promocode);
            customers.ForEach(customer =>
            {
                var promoCodes = customer.PromoCodes?.ToList();
                promoCodes ??= new List<PromoCode>();
                promoCodes.Add(promocode);
                customer.PromoCodes = promoCodes;
                //_dataContext.Entry(customer).State = EntityState.Modified;

            });
            _dataContext.SaveChanges();

        }
    }
}
