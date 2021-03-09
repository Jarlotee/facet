using System;

namespace Facet.Tests.Unit.Dependencies
{
    public class DateTimeReporterWithLog : IDateTimeReporter
    {
        [Facets.Logger]
        public DateTime Report()
        {
            return DateTime.Now;
        }
    }
}
