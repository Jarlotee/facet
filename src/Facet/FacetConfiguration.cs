using System.Collections.Generic;
using System.Reflection;

namespace Facet
{
    public class FacetConfiguration
    {
        public Facet Facet { get; set; }
        public bool IsClass { get; set; }
        public IList<MethodInfo> Methods { get; set; }

        public FacetConfiguration()
        {
            Methods = new List<MethodInfo>();
        }
    }
}
