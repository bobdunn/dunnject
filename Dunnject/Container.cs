using System;
using System.Collections.Generic;

namespace Dunnject
{
    public class Container
    {
        Dictionary<Type, object> types = new Dictionary<Type, object>();

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
            switch (lifecycleType)
            {
                case LifecycleType.Transient:
                    types.Add(typeof(TAbstract), typeof(TConcrete));
                    break;
                case LifecycleType.Singleton:
                    types.Add(typeof(TAbstract), Activator.CreateInstance<TConcrete>());
                    break;
            }
        }

        public T Resolve<T>()
        {
            if (!types.ContainsKey(typeof(T)))
            {
                throw new TypeLoadException();
            }
            if (types[typeof(T)] != null && !(types[typeof(T)] is Type))
            {
                return (T)types[typeof(T)];
            }
            return GetInstance<T>();
        }

        private T GetInstance<T>(){
            return (T)Activator.CreateInstance((Type)types[typeof(T)]);
        }

    }
}
