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

    }

    public class DependentClass
    {
        public DependentClass(IDependency dependency)
        {

        }
    }
    public interface IDependency { }
}
