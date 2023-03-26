using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Example
{
    internal class ExampleSchemaFilter : ISchemaFilter
    {
        private readonly JsonSerializerSettings _settings = new()
        {

            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };


        private static readonly ProblemDetails ProblemDetailsObject = new()
        {
            Type = "Microsoft.AspNetCore.Http.BadHttpRequestException",
            Title = "One or more validation errors occurred",
            Status = 400
        };

        private static readonly GivePromoCodeRequest GivePromoCodeRequestObject = new()
        {
            PartnerName = "asd",
            ServiceInfo = "dsa",
            PromoCode = "new",
            Preference= "Семья"
        };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            object example = context.Type.Name switch
            {
                nameof(ProblemDetails) => ProblemDetailsObject,
                nameof(GivePromoCodeRequest) => GivePromoCodeRequestObject,
                _ => null
            };

            if (example is not null)
                schema.Example = new OpenApiString(JsonConvert.SerializeObject(example, _settings));
        }
    }
}