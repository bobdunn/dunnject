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
            TypeContainer type;
            if (!types.TryGetValue(typeof(T), out type))
            {
                throw new TypeLoadException();
            }

            if (type.Lifecycle == LifecycleType.Singleton)
            {
                if (type.Instance == null)
                {
                    type.Instance = GetInstance<T>();
                }
                return (T)type.Instance;
            }

            return GetInstance<T>();
        }

        private T GetInstance<T>()
        {
            var type = types[typeof(T)];
            if(type.GetDependencies().Any(t => !types.ContainsKey(t))){
                throw new TypeLoadException();
            }
            return (T)Activator.CreateInstance(types[typeof(T)].ConcreteType);
        }

    }
}