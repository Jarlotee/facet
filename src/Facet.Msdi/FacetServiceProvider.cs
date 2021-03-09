using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Facet.Msdi
{
    public class FacetServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _provider;

        public FacetServiceProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public object GetService(Type serviceType)
        {
            var implementation = _provider.GetService(serviceType);

            if (implementation == null)
            {
                return implementation;
            }

            var implementationType = implementation.GetType();

            if (!HasAspectAttribute(implementationType))
            {
                return implementation;
            }

            return BuildAspectChain(serviceType, implementationType, implementation);
        }

        private bool HasAspectAttribute(Type t)
        {
            var classAttribute = Attribute.GetCustomAttribute(t, typeof(Facet));

            if (classAttribute != null)
            {
                return true;
            }

            foreach (var method in t.GetMethods())
            {
                var methodAttribute = Attribute.GetCustomAttribute(method, typeof(Facet));

                if (methodAttribute != null)
                {
                    return true;
                }
            }

            return false;
        }

        private object BuildAspectChain(Type serviceType, Type implementationType, object implementation)
        {
            var chain = GetAspectsChain(serviceType, implementationType);

            var next = implementation;

            while (chain.Count > 0)
            {
                var createMethod = typeof(FacetProxy<>)
                    .MakeGenericType(serviceType)
                    .GetMethod(nameof(FacetProxy<object>.Create), BindingFlags.Public | BindingFlags.Static);

                var proxy = createMethod.Invoke(null, new object[] { next, chain.Dequeue() });
                next = proxy;
            }

            return next;
        }

        private Queue<FacetConfiguration> GetAspectsChain(Type serviceType, Type implementationType)
        {
            var configurations = new Queue<FacetConfiguration>();
            var interfaceMap = implementationType.GetInterfaceMap(serviceType);

            foreach (var attribute in Attribute.GetCustomAttributes(implementationType, typeof(Facet)))
            {
                var config = UpsertAspectConfiguration(ref configurations, attribute);
                config.IsClass = true;
            }

            foreach (var method in implementationType.GetMethods())
            {
                foreach (var attribute in Attribute.GetCustomAttributes(method, typeof(Facet)))
                {
                    var config = UpsertAspectConfiguration(ref configurations, attribute);
                    var methodIndex = Array.IndexOf(interfaceMap.TargetMethods, method);
                    config.Methods.Add(interfaceMap.InterfaceMethods[methodIndex]);
                }
            }

            return configurations;
        }

        private FacetConfiguration UpsertAspectConfiguration(ref Queue<FacetConfiguration> queue, Attribute attribute)
        {
            var facet = queue.Where(a => a.Facet.Equals(attribute)).FirstOrDefault();

            if (facet == null)
            {
                facet = new FacetConfiguration
                {
                    Facet = (Facet)attribute
                };

                queue.Enqueue(facet);
            }

            return facet;
        }
    }
}
