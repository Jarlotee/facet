using System;
using Microsoft.Extensions.DependencyInjection;

namespace Facet.Msdi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildWithFacets(this IServiceCollection collection)
        {
            return new FacetServiceProvider(collection.BuildServiceProvider());
        }
    }
}
