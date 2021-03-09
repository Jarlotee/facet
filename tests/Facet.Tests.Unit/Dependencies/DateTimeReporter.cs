using System;

namespace Facet.Tests.Unit.Dependencies
{
    public class DateTimeReporter : IDateTimeReporter
    {
        public DateTime Report()
        {
            return DateTime.Now;
        }
    }
}
