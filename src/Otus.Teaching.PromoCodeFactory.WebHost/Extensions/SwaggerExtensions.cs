using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Example
{
    internal static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.CustomOperationIds(e => $"{Regex.Replace(e.RelativePath, "{|}", "")}{e.HttpMethod}");
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PromoCodeFactory", Version = "v1" });
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
                options.SchemaFilter<ExampleSchemaFilter>();

            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }
    }
}