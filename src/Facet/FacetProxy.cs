using System.Linq;
using System.Reflection;

namespace Facet
{
    public class FacetProxy<T> : DispatchProxy
    {
        private FacetConfiguration _configuration;
        private T _target;


        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var shouldProxy = ShouldMethodBeProxied(targetMethod);

            if(!shouldProxy)
            {
                return targetMethod.Invoke(_target, args);
            }

            return _configuration.Facet.Handle(targetMethod, args, _target);
        }

        private bool ShouldMethodBeProxied(MethodInfo targetMethod)
        {
            if(_configuration.IsClass)
            {
                return true;
            }

            if(_configuration.Methods.Any(m => m.Equals(targetMethod)))
            {
                return true;
            }

            return false;
        }

        public static T Create(T target, FacetConfiguration configuration)
        {
            object proxy = Create<T, FacetProxy<T>>();

            ((FacetProxy<T>) proxy).SetParamters(target, configuration);

            return (T)proxy;
        }

        public void SetParamters(T target, FacetConfiguration configuration)
        {
            _configuration = configuration;
            _target = target;
        }
    }
}
