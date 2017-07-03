using System.Collections;
using Xunit;

namespace Dunnject.Tests
{
    public class When_container_is_initialized
    {
        Container container;

        public When_container_is_initialized()
        {
            container = new Container();
        }

        [Fact]
        public void it_can_enumerate_registered_types()
        {
            var types = container.GetRegisteredTypes();
            Assert.IsAssignableFrom<IEnumerable>(types);
        }

        [Fact]
        public void it_can_register_a_simple_type()
        {
            container.RegisterType<SampleClass>();
        }
    }

    public class SampleClass { }
}
