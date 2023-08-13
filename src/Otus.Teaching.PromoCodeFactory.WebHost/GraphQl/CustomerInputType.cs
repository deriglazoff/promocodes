using GraphQL.Types;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace MyHotel.GraphQL
{
    public class CustomerInputType : InputObjectGraphType<CreateOrEditCustomerRequest>
    {
        public CustomerInputType()
        {
            Field(d => d.LastName, nullable: true);
            Field(d => d.FirstName, nullable: true);
            Field(d => d.Email, nullable: true);
            Field(d => d.PreferenceIds, nullable: false);
        }
    }
}

