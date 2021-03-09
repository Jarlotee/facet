using System;
using Facet.Msdi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Facet.Tests.Unit
{
    public class Interception
    {
        private readonly IServiceProvider _provider;

        public Interception()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<Dependencies.IDateTimeReporter, Dependencies.DateTimeReporterWithLog>();

            _provider = colllection.BuildWithFacets();
        }

        [Fact]
        public void DoesIntercept()
        {
            var reporter = _provider.GetService<Dependencies.IDateTimeReporter>();
            var result = reporter.Report();

            Assert.IsType<DateTime>(result);
            Assert.True(DateTime.Now > result);
        }

    }
}
