using System;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable InvalidXmlDocComment

namespace Sphyrnidae.Common.Utilities
{
    /// <summary>
    /// Obtains a service registered in the DI framework
    /// </summary>
    /// <remarks>
    /// .net Core no longer supports service locator in the same fashion that .net framework did.
    /// Eg. in .net FW, you could directly access the global DependencyResolver and it would give you the correct thing.
    /// In .net Core, you can't directly access this, and an attempt to "save it off" fails as well.
    /// This fails because this is saving off the root scope, which effectively turns scoped items into singletons.
    /// I have found no other way than to unfortunately rely solely on DI - either injecting the services directly,
    /// Or injecting IServiceProvider and doing a lookup that way.
    /// Either way, you have to DI something, so you might as well DI things directly instead of IServiceProvider.
    /// But I'll leave this method here in case you only want to pass around 1 object instead of everything.
    /// </remarks>
    /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.0#scope-validation"/>
    public static class ServiceLocator
    {
        /*
        private static IServiceProvider _serviceProvider;
        public static void Initialize(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
        public static T Get<T>() => (_serviceProvider ?? throw new Exception("Service Provider not initialized")).GetRequiredService<T>();
        */
        public static T Get<T>(IServiceProvider sp) => (sp ?? throw new Exception("Service Provider not provided")).GetRequiredService<T>();
    }
}