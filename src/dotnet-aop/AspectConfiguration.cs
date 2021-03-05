using System.Collections.Generic;
using System.Reflection;

namespace dotnet_aop
{
    public class AspectConfiguration
    {
        public Aspect Aspect { get; set; }
        public bool IsClass { get; set; }
        public IList<MethodInfo> Methods { get; set; }

        public AspectConfiguration()
        {
            Methods = new List<MethodInfo>();
        }
    }
}
