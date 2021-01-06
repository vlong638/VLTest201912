using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    public class AuthHeaderFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "VLSession",
                In = ParameterLocation.Header,
                Required = true,
                Description = "Authorization",
                Schema = new OpenApiSchema { Type = "string" }
            }); 
        }
    }
}
