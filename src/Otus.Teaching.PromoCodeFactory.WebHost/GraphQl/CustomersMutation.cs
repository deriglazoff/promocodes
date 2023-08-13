using GraphQL;
using GraphQL.Types;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;

namespace MyHotel.GraphQL
{
    public class CustomersMutation : ObjectGraphType
    {
        [Obsolete]
        public CustomersMutation(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {

            Field<CustomerType>(
                "UpdateCustomer",
                                arguments:
                new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    },
                    new QueryArgument<CustomerInputType> { Name = "customer" }
                }),
                resolve: context =>
                {
                    var request = context.GetArgument<CreateOrEditCustomerRequest>("customer");
                    //Получаем предпочтения из бд и сохраняем большой объект
                    var preferences = preferenceRepository
                        .GetRangeByIdsAsync(request.PreferenceIds).Result;

                    Customer customer = CustomerMapper.MapFromModel(request, preferences);

                    var reservationId = context.GetArgument<Guid?>("id");
                    if (reservationId.HasValue)
                    {
                        customerRepository.UpdateAsync(customer).Wait();
                        customer.Id = reservationId.Value;
                    }
                    else
                    {
                        customerRepository.AddAsync(customer).Wait();
                    }
                    return customerRepository.GetByIdAsync(customer.Id).Result;
                });

            Field<CustomerType>(
                "DeleteCustomer",
                arguments:
                new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    }
                }),
                resolve: context =>
                {
                    var reservationId = context.GetArgument<Guid?>("id");

                    var customer = customerRepository.GetByIdAsync(reservationId.Value).Result;
                    customerRepository.DeleteAsync(customer).Wait();
                    return default;
                });
        }
    }
}

