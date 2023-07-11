using Otus.Teaching.PromoCodeFactory.Core.Exceptions;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.UseCases
{
    public class PartnersCase
    {
        private readonly IRepository<Partner> _partnersRepository;

        public PartnersCase(
            IRepository<Partner> partnersRepository)
        {
            _partnersRepository = partnersRepository;
        }

        public async Task<PartnerResponse> GetPartnerByIdAsync(Guid id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id) ?? throw new NotFoundException("Партнер не найден");
            if (partner == null) return null;

            return new PartnerResponse()
            {
                Id = partner.Id,
                Name = partner.Name,
                NumberIssuedPromoCodes = partner.NumberIssuedPromoCodes,
                IsActive = true,
                PartnerLimits = partner.PartnerLimits
                    .Select(y => new PartnerPromoCodeLimitResponse()
                    {
                        Id = y.Id,
                        PartnerId = y.PartnerId,
                        Limit = y.Limit,
                        CreateDate = y.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        EndDate = y.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        CancelDate = y.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
                    }).ToList()
            };
        }

        public async Task<List<PartnerResponse>> GetPartnersAsync()
        {
            var partners = await _partnersRepository.GetAllAsync();

            return partners.Select(x => new PartnerResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NumberIssuedPromoCodes = x.NumberIssuedPromoCodes,
                IsActive = true,
                PartnerLimits = x.PartnerLimits
                    .Select(y => new PartnerPromoCodeLimitResponse()
                    {
                        Id = y.Id,
                        PartnerId = y.PartnerId,
                        Limit = y.Limit,
                        CreateDate = y.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        EndDate = y.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        CancelDate = y.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
                    }).ToList()
            }).ToList();
        }

        public async Task<PartnerPromoCodeLimitResponse> GetPartnerLimitAsync(Guid id, Guid limitId)
        {
            var partner = await _partnersRepository.GetByIdAsync(id) ?? throw new NotFoundException("Партнер не найден");

            var limit = partner.PartnerLimits.FirstOrDefault(x => x.Id == limitId);
            if (limit == null) return null;

            return new PartnerPromoCodeLimitResponse()
            {
                Id = limit.Id,
                PartnerId = limit.PartnerId,
                Limit = limit.Limit,
                CreateDate = limit.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                EndDate = limit.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                CancelDate = limit.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss")
            };
        }

        public async Task<(Guid partnerId, Guid newLimitId)> SetPartnerPromoCodeLimitAsync(Guid id, SetPartnerPromoCodeLimitRequest request)
        {
            var partner = await _partnersRepository.GetByIdAsync(id) ?? throw new NotFoundException("Партнер не найден");

            //Если партнер заблокирован, то нужно выдать исключение
            if (!partner.IsActive)
                throw new BadRequestException("Данный партнер не активен");

            //Установка лимита партнеру
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
                !x.CancelDate.HasValue);

            if (activeLimit != null)
            {
                //Если партнеру выставляется лимит, то мы 
                //должны обнулить количество промо-кодов, которые партнер выдал, если лимит закончился, 
                //то количество не обнуляется
                partner.NumberIssuedPromoCodes = 0;

                //При установке лимита нужно отключить предыдущий лимит
                activeLimit.CancelDate = DateTime.Now;
            }

            if (request.Limit <= 0)
                throw new BadRequestException("Лимит должен быть больше 0");

            var newLimit = CreatePartnerPromoCodeLimit(partner, request.Limit, request.EndDate);

            partner.PartnerLimits.Add(newLimit);

            await _partnersRepository.UpdateAsync(partner);

            return (partner.Id, newLimit.Id);
        }

        private PartnerPromoCodeLimit CreatePartnerPromoCodeLimit(Partner partner, int limit, DateTime endDate)
        {
            return new PartnerPromoCodeLimit()
            {
                Id = Guid.NewGuid(),
                Limit = limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = endDate
            };
        }

        public async Task CancelPartnerPromoCodeLimitAsync(Guid id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id) ?? throw new NotFoundException("Партнер не найден");

            //Если партнер заблокирован, то нужно выдать исключение
            if (!partner.IsActive)
                throw new BadRequestException("Данный партнер не активен");

            //Отключение лимита
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
                !x.CancelDate.HasValue);

            if (activeLimit != null)
            {
                activeLimit.CancelDate = DateTime.Now;

                await _partnersRepository.UpdateAsync(partner);
            }
        }
    }
}
