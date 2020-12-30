using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Stize.ApiTemplate.Api.Host.Configuration.Swagger
{
    public class SwaggerOptionsConfigure : IConfigureOptions<SwaggerOptions>
    {
        public void Configure(SwaggerOptions options)
        {            
            options.PreSerializeFilters.Add((apiDocument, httpReq) =>
            {
                
            });
        }
    }
}