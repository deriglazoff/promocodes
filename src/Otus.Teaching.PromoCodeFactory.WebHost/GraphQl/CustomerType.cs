using GraphQL.Types;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace MyHotel.GraphQL
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType()
        {
            Field(d => d.Id, nullable: false);
            Field(d => d.LastName, nullable: true);
            Field(d => d.FirstName, nullable: true);
            Field(d => d.Email, nullable: true);
            Field(d => d.FullName, nullable: true);
        }
    }
}

