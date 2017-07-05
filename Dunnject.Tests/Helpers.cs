namespace Dunnject.Tests
{
    public class SampleClass { }
    public class NotRegistered { }
    public class UseAsSingleton { }
    public class DependentClass
    {
        public DependentClass(IDependency dependency) { }
    }
    public interface IDependency { }
    public class Dependency : IDependency { }
}