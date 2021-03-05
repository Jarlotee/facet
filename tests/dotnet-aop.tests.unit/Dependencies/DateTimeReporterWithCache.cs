using System;

namespace dotnet_aop.tests.unit.dependencies
{
    public class DateTimeReporterWithLog : IDateTimeReporter
    {
        [aspects.Logger]
        public DateTime Report()
        {
            return DateTime.Now;
        }
    }
}
