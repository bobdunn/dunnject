using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dunnject.Tests
{
    public class When_container_is_initialized
    {
        Container container;

        public When_container_is_initialized()
        {
            container = new Container();
            container.RegisterType<SampleClass>();
        }

        [Fact]
        public void it_can_enumerate_registered_types()
        {
            var types = container.GetRegisteredTypes();
            Assert.IsAssignableFrom<System.Collections.IEnumerable>(types);
        }

        [Fact]
        public void it_can_register_a_simple_type()
        {
            var types = container.GetRegisteredTypes();
            Assert.Equal(typeof(SampleClass), types.First());
        }

        [Fact]
        public void it_can_get_a_simple_type()
        {
            var sampleClass = container.Resolve<SampleClass>();
            Assert.IsType<SampleClass>(sampleClass);
        }

        [Fact]
        public void it_throws_TypeLoadException_trying_to_resolve_unregistered_class()
        {
            Assert.Throws<TypeLoadException>(() => container.Resolve<NotRegistered>());
        }

        [Fact]
        public void it_returns_different_instance_for_transient_lifecycle()
        {
            var firstInstance = container.Resolve<SampleClass>();
            var secondInstance = container.Resolve<SampleClass>();
            Assert.NotSame(firstInstance, secondInstance);
        }

        [Fact]
        public void it_returns_same_instance_for_singleton()
        {
            container.RegisterType<UseAsSingleton>(LifecycleType.Singleton);
            var firstInstance = container.Resolve<UseAsSingleton>();
            var secondInstance = container.Resolve<UseAsSingleton>();
            Assert.Same(firstInstance, secondInstance);
        }
    }

    public class SampleClass { }
    public class NotRegistered { }
    public class UseAsSingleton { }

}
