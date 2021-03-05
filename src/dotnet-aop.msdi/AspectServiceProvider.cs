using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnet_aop.msdi
{
    public class AspectServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _provider;

        public AspectServiceProvider(IServiceProvider provider)
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
            var classAttribute = Attribute.GetCustomAttribute(t, typeof(Aspect));

            if (classAttribute != null)
            {
                return true;
            }

            foreach (var method in t.GetMethods())
            {
                var methodAttribute = Attribute.GetCustomAttribute(method, typeof(Aspect));

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
                var createMethod = typeof(AspectProxy<>)
                    .MakeGenericType(serviceType)
                    .GetMethod(nameof(AspectProxy<object>.Create), BindingFlags.Public | BindingFlags.Static);

                var proxy = createMethod.Invoke(null, new object[] { next, chain.Dequeue() });
                next = proxy;
            }

            return next;
        }

        private Queue<AspectConfiguration> GetAspectsChain(Type serviceType, Type implementationType)
        {
            var aspects = new Queue<AspectConfiguration>();
            var interfaceMap = implementationType.GetInterfaceMap(serviceType);

            foreach (var attribute in Attribute.GetCustomAttributes(implementationType, typeof(Aspect)))
            {
                var config = UpsertAspectConfiguration(ref aspects, attribute);
                config.IsClass = true;
            }

            foreach (var method in implementationType.GetMethods())
            {
                foreach (var attribute in Attribute.GetCustomAttributes(method, typeof(Aspect)))
                {
                    var config = UpsertAspectConfiguration(ref aspects, attribute);
                    var methodIndex = Array.IndexOf(interfaceMap.TargetMethods, method);
                    config.Methods.Add(interfaceMap.InterfaceMethods[methodIndex]);
                }
            }

            return aspects;
        }

        private AspectConfiguration UpsertAspectConfiguration(ref Queue<AspectConfiguration> queue, Attribute attribute)
        {
            var aspect = queue.Where(a => a.Aspect.Equals(attribute)).FirstOrDefault();

            if (aspect == null)
            {
                aspect = new AspectConfiguration
                {
                    Aspect = (Aspect)attribute
                };

                queue.Enqueue(aspect);
            }

            return aspect;
        }
    }
}
