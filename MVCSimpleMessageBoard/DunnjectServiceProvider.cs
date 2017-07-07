using Dunnject;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MVCSimpleMessageBoard
{
    public class DunnjectServiceProvider : IServiceProvider
    {
        Container container;
        public DunnjectServiceProvider(IServiceCollection serviceCollection)
        {
            container = new Container();
            RegisterFrameworkServices(serviceCollection);
            ConfigureProvider();
        }

        private void RegisterFrameworkServices(IServiceCollection serviceCollection)
        {
            foreach (var service in serviceCollection)
            {
                if (service.Lifetime == ServiceLifetime.Singleton)
                {
                    if (service.ImplementationInstance != null)
                    {
                        container.RegisterType(service.ServiceType, service.ImplementationInstance);
                    }
                    else
                    {
                        container.RegisterType(service.ServiceType, service.ImplementationType, LifecycleType.Singleton);
                    }
                }
                else
                {
                    container.RegisterType(service.ServiceType, service.ImplementationType);
                }
            }
        }

        string GetName(Type type)
        {
            return $"{type.Namespace}.{type.Name}";
        }

        private void ConfigureProvider()
        {
            container.RegisterType<IMessageContainer, MessageContainer>(LifecycleType.Singleton);
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }
    }
}