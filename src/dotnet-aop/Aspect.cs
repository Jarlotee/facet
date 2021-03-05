using System;
using System.Reflection;
using System.Threading.Tasks;

namespace dotnet_aop
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class Aspect : Attribute
    {
        public abstract object Handle(MethodInfo targetMethod, object[] args, object target);
    }
}
