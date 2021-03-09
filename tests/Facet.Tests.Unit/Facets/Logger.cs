using System;
using System.Reflection;

namespace Facet.Tests.Unit.Facets
{
    public class Logger : Facet
    {
        public override object Handle(MethodInfo targetMethod, object[] args, object target)
        {
            var result = targetMethod.Invoke(target, args);

            Console.WriteLine($"Logger Aspect: {result}");

            return result;
        }
    }
}
