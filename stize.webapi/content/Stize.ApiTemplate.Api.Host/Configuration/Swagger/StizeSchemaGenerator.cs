using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Stize.Domain.Model;
using Stize.DotNet.Delta;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stize.ApiTemplate.Api.Host.Configuration.Swagger
{
    public class StizeSchemaGenerator : ISchemaGenerator
    {
        private readonly SchemaGenerator defaultSchemaGenerator;

        public StizeSchemaGenerator(SchemaGeneratorOptions generatorOptions, ISerializerDataContractResolver serializerDataContractResolver)
        {
            this.defaultSchemaGenerator = new SchemaGenerator(generatorOptions, serializerDataContractResolver);
        }

        public OpenApiSchema GenerateSchema(
            Type modelType,
            SchemaRepository schemaRepository,
            MemberInfo memberInfo = null,
            ParameterInfo parameterInfo = null,
            ApiParameterRouteInfo routeInfo = null)
        {
            if (modelType.IsConstructedGenericType && Delta.IsDelta(modelType))
            {
                var deltaTpe = modelType.GetGenericArguments()[0];
                var result = this.defaultSchemaGenerator.GenerateSchema(deltaTpe, schemaRepository, memberInfo, parameterInfo);
                return result;
            }

            if (typeof(MultiUploadedFileModel).IsAssignableFrom(modelType))
            {
                var formType = typeof(IFormFile);
                var result = this.defaultSchemaGenerator.GenerateSchema(formType, schemaRepository, memberInfo, parameterInfo);
                result.Format = nameof(MultiUploadedFileModel);
                return result;
            }

            return this.defaultSchemaGenerator.GenerateSchema(modelType, schemaRepository, memberInfo, parameterInfo);
        }

    }
}
