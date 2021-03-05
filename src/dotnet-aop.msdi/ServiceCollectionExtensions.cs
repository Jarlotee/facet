using System;
using Microsoft.Extensions.DependencyInjection;

namespace dotnet_aop.msdi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildWithAspects(this IServiceCollection collection)
        {
            return new AspectServiceProvider(collection.BuildServiceProvider());
        }
    }
}
