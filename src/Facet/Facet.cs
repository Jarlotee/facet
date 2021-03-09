using System;
using System.Reflection;

namespace Facet
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class Facet : Attribute
    {
        public abstract object Handle(MethodInfo targetMethod, object[] args, object target);
    }
}
