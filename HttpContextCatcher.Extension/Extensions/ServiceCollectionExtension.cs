using HttpContextCatcher.Extension.Converters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IMvcBuilder AddBsonSerializer(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new BsonValueJsonConverter());
            });

            return builder;
        }
    }
}
