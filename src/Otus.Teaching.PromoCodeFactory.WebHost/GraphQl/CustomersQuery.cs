using GraphQL;
using GraphQL.Types;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHotel.GraphQL
{

    public class CustomersQuery : ObjectGraphType
    {
        [Obsolete]
        public CustomersQuery(IRepository<Customer> customerRepository)
        {
            
            Field<ListGraphType<CustomerType>>("Customers",
                arguments:
                new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    },
                }),
                resolve: context =>
            {
                var customers = customerRepository.GetAllAsync().Result;

                var reservationId = context.GetArgument<Guid?>("id");
                if (reservationId.HasValue)
                {
                    return customers.Where(x=>x.Id == reservationId.Value);
                }

                return customers;
            });
        }
    }
}

