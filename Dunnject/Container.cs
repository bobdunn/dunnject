using System;
using System.Collections.Generic;
using System.Linq;

namespace Dunnject
{
    public class Container
    {
        Dictionary<Type, TypeContainer> types = new Dictionary<Type, TypeContainer>();

        public IEnumerable<Type> GetRegisteredTypes()
        {
            return types.Keys;
        }

        public void RegisterType<T>(LifecycleType lifecycleType = LifecycleType.Transient)
        {
            RegisterType<T, T>(lifecycleType);
        }

        public void RegisterType<TAbstract, TConcrete>(LifecycleType lifecycleType = LifecycleType.Transient)
        {
            types.Add(typeof(TAbstract), new TypeContainer(typeof(TAbstract), typeof(TConcrete), lifecycleType));
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        private object Resolve(Type type)
        {
            TypeContainer typeContainer;
            if (!types.TryGetValue(type, out typeContainer))
            {
                throw new TypeLoadException();
            }

            if (typeContainer.Lifecycle == LifecycleType.Singleton)
            {
                if (typeContainer.Instance == null)
                {
                    typeContainer.Instance = GetInstance(type);
                }
                return typeContainer.Instance;
            }

            return GetInstance(type);
        }

        private HashSet<Type> resolvingTypes = new HashSet<Type>();
        private object GetInstance(Type type)
        {
            var typeContainer = types[type];
            var args = typeContainer.GetDependencies().Select(d =>
            {
                if (types.ContainsKey(d) && !resolvingTypes.Contains(d))
                {
                    resolvingTypes.Add(d);
                    var dependentType = Resolve(d);
                    resolvingTypes.Remove(d);
                    return dependentType;
                }
                else { throw new TypeLoadException(); }
            }).ToArray();

            return Activator.CreateInstance(typeContainer.ConcreteType, args);
        }
    }
}