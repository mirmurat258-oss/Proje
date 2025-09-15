using Microsoft.AspNetCore.OpenApi;

namespace API.Conf;

public static class OpenApiExtensions
{
    public static OpenApiOptions CustomSchemaIds(this OpenApiOptions config,
        Func<Type, string?> typeSchemaTransformer,
        bool includeValueTypes = false)
    {
        return config.AddSchemaTransformer((schema, context, _) =>
        {
            if (!includeValueTypes &&
                (context.JsonTypeInfo.Type.IsValueType ||
                 context.JsonTypeInfo.Type == typeof(String) ||
                 context.JsonTypeInfo.Type == typeof(string)))
            {
                return Task.CompletedTask;
            }

            if (schema.Annotations == null || !schema.Annotations.TryGetValue("x-schema-id", out object? _))
            {
                return Task.CompletedTask;
            }

            string? transformedTypeName = typeSchemaTransformer(context.JsonTypeInfo.Type);
            schema.Annotations["x-schema-id"] = transformedTypeName;

            schema.Title = transformedTypeName;

            return Task.CompletedTask;
        });
    }
}