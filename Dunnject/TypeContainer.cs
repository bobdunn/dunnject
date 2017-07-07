using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Dunnject
{
    public class TypeContainer
    {

        public LifecycleType Lifecycle { get; set; }
        public object Instance { get; set; }
        public Type ConcreteType { get; set; }
        public Type AbstractType { get; set; }

        public TypeContainer(Type abstractType, Type concreteType, LifecycleType lifecycle = LifecycleType.Transient)
        {
            this.AbstractType = abstractType;
            this.ConcreteType = concreteType;
            Lifecycle = lifecycle;
        }

        public TypeContainer(Type abstractType, object instance)
        {
            this.AbstractType = abstractType;
            this.ConcreteType = abstractType;
            Lifecycle = LifecycleType.Singleton;
            Instance = instance;
        }

        public IEnumerable<Type> GetDependencies()
        {
            var constructor = ConcreteType.GetTypeInfo().DeclaredConstructors.OrderBy(x=>x.GetParameters().Length).First();
            return constructor.GetParameters().Select(x => x.ParameterType);
        }
    }
}
