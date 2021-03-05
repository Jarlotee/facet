using System;

namespace dotnet_aop.tests.unit.dependencies
{
    public class DateTimeReporter : IDateTimeReporter
    {
        public DateTime Report()
        {
            return DateTime.Now;
        }
    }
}
