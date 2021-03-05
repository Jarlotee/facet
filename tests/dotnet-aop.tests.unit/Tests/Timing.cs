using System;
using dotnet_aop.msdi;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace dotnet_aop.tests.unit
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

            colllection.AddSingleton<dependencies.IDateTimeReporter, dependencies.DateTimeReporter>();

            return colllection.BuildServiceProvider();
        }

        private IServiceProvider CreateStandardServiceProviderWithAspects()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<dependencies.IDateTimeReporter, dependencies.DateTimeReporter>();

            return colllection.BuildWithAspects();
        }

        private IServiceProvider CreateStandardServiceProviderWithAspectsImplemented()
        {
            var colllection = new ServiceCollection();

            colllection.AddSingleton<dependencies.IDateTimeReporter, dependencies.DateTimeReporterWithLog>();

            return colllection.BuildWithAspects();
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
            baseProvider.GetService<dependencies.IDateTimeReporter>();
            baseline.Stop();

            withAspects.Start();
            aspectProvider.GetService<dependencies.IDateTimeReporter>();
            withAspects.Stop();

            withAspectsImplemented.Start();
            aspectImplementedProvider.GetService<dependencies.IDateTimeReporter>();
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

            baseProvider.GetService<dependencies.IDateTimeReporter>();
            aspectProvider.GetService<dependencies.IDateTimeReporter>();
            aspectImplementedProvider.GetService<dependencies.IDateTimeReporter>();

            baseline.Start();
            baseProvider.GetService<dependencies.IDateTimeReporter>();
            baseline.Stop();

            withAspects.Start();
            aspectProvider.GetService<dependencies.IDateTimeReporter>();
            withAspects.Stop();

            withAspectsImplemented.Start();
            aspectImplementedProvider.GetService<dependencies.IDateTimeReporter>();
            withAspectsImplemented.Stop();

            Console.WriteLine($"GetServiceTimingWarm Execution Time (Baseline) {baseline.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"GetServiceTimingWarm Execution Time (With Aspects) {withAspects.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"GetServiceTimingWarm Execution Time (With Aspects Implemented) {withAspectsImplemented.Elapsed.TotalMilliseconds} ms");
        }

    }
}
