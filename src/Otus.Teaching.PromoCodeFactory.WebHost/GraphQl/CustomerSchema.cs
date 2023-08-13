using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyHotel.GraphQL
{
    public class CustomerSchema : Schema
    {
        public CustomerSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<CustomersQuery>();
            Mutation = serviceProvider.GetRequiredService<CustomersMutation>();
        }
    }
}

