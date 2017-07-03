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
            switch (lifecycleType)
            {
                case LifecycleType.Transient:
                    types.Add(typeof(T), null);
                    break;
                case LifecycleType.Singleton:
                    types.Add(typeof(T), GetInstance<T>());
                    break;
            }
        }

        public T Resolve<T>()
        {
            if (!types.ContainsKey(typeof(T)))
            {
                throw new TypeLoadException();
            }
            if (types[typeof(T)] != null)
            {
                return (T)types[typeof(T)];
            }
            return GetInstance<T>();
        }

        private T GetInstance<T>(){
            return Activator.CreateInstance<T>();
        }
    }
}
