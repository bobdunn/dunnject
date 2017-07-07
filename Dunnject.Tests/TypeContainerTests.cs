using System;
using Xunit;

namespace Dunnject.Tests
{
    public class When_using_a_TypeContainer
    {
        [Fact]
        public void it_should_expose_required_dependencies()
        {
            var typeContainer = new TypeContainer(typeof(DependentClass), typeof(DependentClass));
            Assert.Contains(typeof(IDependency), typeContainer.GetDependencies());
        }

        [Fact]
        public void it_should_default_to_transient_lifecycle()
        {
            var typeContainer = new TypeContainer(typeof(SampleClass), typeof(SampleClass));
            Assert.Equal(LifecycleType.Transient, typeContainer.Lifecycle);
        }
    }

    public class When_using_a_singleton_TypeContainer
    {
        [Fact]
        public void it_should_allow_you_to_provide_the_instance()
        {
            var sampleClass = new SampleClass();
            var typeContainer = new TypeContainer(typeof(SampleClass), sampleClass);
            Assert.Same(sampleClass, typeContainer.Instance);
        }
    }


}
