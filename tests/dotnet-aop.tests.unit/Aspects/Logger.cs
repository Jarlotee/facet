using System;
using System.Reflection;

namespace dotnet_aop.tests.unit.aspects
{
    public class Logger : Aspect
    {
        public override object Handle(MethodInfo targetMethod, object[] args, object target)
        {
            var result = targetMethod.Invoke(target, args);

            Console.WriteLine($"Logger Aspect: {result}");

            return result;
        }
    }
}
