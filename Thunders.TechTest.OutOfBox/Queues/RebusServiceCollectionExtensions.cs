using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using System.Diagnostics;
using System.Reflection;

namespace Thunders.TechTest.OutOfBox.Queues
{
    public static class RebusServiceCollectionExtensions
    {
        public static IServiceCollection AddBus(
            this IServiceCollection services,
            string? connectionString,
            SubscriptionBuilder? subscriptionBuilder = null)
        {
            services.AutoRegisterHandlersFromAssembly(Assembly.GetEntryAssembly());
            services.AddRebus(c => c
                .Transport(t =>
                {
                    t.UseRabbitMq(connectionString, "Thunders.TechTest");
                })
                 .Options(o =>
                 {
                     o.SetNumberOfWorkers(12);
                     o.SetMaxParallelism(12);
                 })
                .Logging(configurer => configurer.ColoredConsole())
                , 
                onCreated: async bus =>
                {
                    if (subscriptionBuilder != null)
                        foreach (var type in subscriptionBuilder.TypesToSubscribe)
                            await bus.Subscribe(type);
                });

            return services;
        }
    }

    public class SubscriptionBuilder
    {
        internal List<Type> TypesToSubscribe { get; private set; } = [];

        public SubscriptionBuilder Add<T>()
        {
            TypesToSubscribe.Add(typeof(T));

            return this;
        }
    }
}
