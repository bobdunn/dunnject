using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Dunnject
{
    public class TypeContainer
    {
        private Type abstractType;
        private Type concreteType;

        public TypeContainer(Type abstractType, Type concreteType, LifecycleType lifecycle = LifecycleType.Transient)
        {
            this.abstractType = abstractType;
            this.concreteType = concreteType;
            Lifecycle = lifecycle;
        }

        public LifecycleType Lifecycle { get; set; }

        public IEnumerable<Type> GetDependencies()
        {
            var constructor = concreteType.GetTypeInfo().DeclaredConstructors.OrderBy(x=>x.GetParameters().Length).First();
            return constructor.GetParameters().Select(x => x.ParameterType);
        }
    }
}
