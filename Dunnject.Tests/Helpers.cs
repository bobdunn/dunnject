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
    public class CircularA
    {
        public CircularA(CircularB circularB) { }
    }
    public class CircularB
    {
        public CircularB(CircularA circularA) { }
    }
}