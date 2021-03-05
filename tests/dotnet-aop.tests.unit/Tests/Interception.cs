using System;
using dotnet_aop.msdi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace dotnet_aop.tests.unit
{
    public class Interception
    {
        private readonly IServiceProvider _provider;

        public Interception()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<dependencies.IDateTimeReporter, dependencies.DateTimeReporterWithLog>();

            _provider = colllection.BuildWithAspects();
        }

        [Fact]
        public void DoesIntercept()
        {
            var reporter = _provider.GetService<dependencies.IDateTimeReporter>();
            var result = reporter.Report();

            Assert.IsType<DateTime>(result);
            Assert.True(DateTime.Now > result);
        }

    }
}
