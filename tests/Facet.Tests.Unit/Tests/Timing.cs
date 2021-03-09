using System;
using Facet.Msdi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Facet.Tests.Unit
{
    public class Timing
    {
        public Timing()
        {
            // handle unrelated loading by running this before the tests
            CreateStandardServiceProvider();
        }

        private IServiceProvider CreateStandardServiceProvider()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<Dependencies.IDateTimeReporter, Dependencies.DateTimeReporter>();

            return colllection.BuildServiceProvider();
        }

        private IServiceProvider CreateStandardServiceProviderWithAspects()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<Dependencies.IDateTimeReporter, Dependencies.DateTimeReporter>();

            return colllection.BuildWithFacets();
        }

        private IServiceProvider CreateStandardServiceProviderWithAspectsImplemented()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<Dependencies.IDateTimeReporter, Dependencies.DateTimeReporterWithLog>();

            return colllection.BuildWithFacets();
        }

        [Fact]
        public void BuildServiceProvider()
        {
            var baseline = new System.Diagnostics.Stopwatch();
            var withAspects = new System.Diagnostics.Stopwatch();

            baseline.Start();
            CreateStandardServiceProvider();
            baseline.Stop();

            withAspects.Start();
            CreateStandardServiceProviderWithAspects();
            withAspects.Stop();

            Console.WriteLine($"BuildServiceProvider Execution Time (Baseline) {baseline.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"BuildServiceProvider Execution Time (With Aspects) {withAspects.Elapsed.TotalMilliseconds} ms");
        }

        [Fact]
        public void GetServiceTimingCold()
        {
            var baseline = new System.Diagnostics.Stopwatch();
            var withAspects = new System.Diagnostics.Stopwatch();
            var withAspectsImplemented = new System.Diagnostics.Stopwatch();

            var baseProvider = CreateStandardServiceProvider();
            var aspectProvider = CreateStandardServiceProviderWithAspects();
            var aspectImplementedProvider = CreateStandardServiceProviderWithAspectsImplemented();

            baseline.Start();
            baseProvider.GetService<Dependencies.IDateTimeReporter>();
            baseline.Stop();

            withAspects.Start();
            aspectProvider.GetService<Dependencies.IDateTimeReporter>();
            withAspects.Stop();

            withAspectsImplemented.Start();
            aspectImplementedProvider.GetService<Dependencies.IDateTimeReporter>();
            withAspectsImplemented.Stop();

            Console.WriteLine($"GetServiceTimingCold Execution Time (Baseline) {baseline.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"GetServiceTimingCold Execution Time (With Aspects) {withAspects.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"GetServiceTimingCold Execution Time (With Aspects Implemented) {withAspectsImplemented.Elapsed.TotalMilliseconds} ms");
        }

        [Fact]
        public void GetServiceTimingWarm()
        {
            var baseline = new System.Diagnostics.Stopwatch();
            var withAspects = new System.Diagnostics.Stopwatch();
            var withAspectsImplemented = new System.Diagnostics.Stopwatch();

            var baseProvider = CreateStandardServiceProvider();
            var aspectProvider = CreateStandardServiceProviderWithAspects();
            var aspectImplementedProvider = CreateStandardServiceProviderWithAspectsImplemented();

            baseProvider.GetService<Dependencies.IDateTimeReporter>();
            aspectProvider.GetService<Dependencies.IDateTimeReporter>();
            aspectImplementedProvider.GetService<Dependencies.IDateTimeReporter>();

            baseline.Start();
            baseProvider.GetService<Dependencies.IDateTimeReporter>();
            baseline.Stop();

            withAspects.Start();
            aspectProvider.GetService<Dependencies.IDateTimeReporter>();
            withAspects.Stop();

            withAspectsImplemented.Start();
            aspectImplementedProvider.GetService<Dependencies.IDateTimeReporter>();
            withAspectsImplemented.Stop();

            Console.WriteLine($"GetServiceTimingWarm Execution Time (Baseline) {baseline.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"GetServiceTimingWarm Execution Time (With Aspects) {withAspects.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"GetServiceTimingWarm Execution Time (With Aspects Implemented) {withAspectsImplemented.Elapsed.TotalMilliseconds} ms");
        }

    }
}
