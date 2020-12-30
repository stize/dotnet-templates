using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
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
            Type type,
            SchemaRepository schemaRepository,
            MemberInfo memberInfo = null,
            ParameterInfo parameterInfo = null)
        {
            if (type.IsConstructedGenericType && Delta.IsDelta(type))
            {
                var deltaTpe = type.GetGenericArguments()[0];
                var result = this.defaultSchemaGenerator.GenerateSchema(deltaTpe, schemaRepository, memberInfo, parameterInfo);
                return result;
            }

            if (typeof(MultiUploadedFileModel).IsAssignableFrom(type))
            {
                var formType = typeof(IFormFile);
                var result = this.defaultSchemaGenerator.GenerateSchema(formType, schemaRepository, memberInfo, parameterInfo);
                result.Format = nameof(MultiUploadedFileModel);
                return result;
            }

            return this.defaultSchemaGenerator.GenerateSchema(type, schemaRepository, memberInfo, parameterInfo);
        }
    }
}
