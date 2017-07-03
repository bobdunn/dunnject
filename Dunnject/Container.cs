using System;
using System.Collections.Generic;

namespace Dunnject
{
    public class Container
    {
        List<Type> types = new List<Type>();

        public IEnumerable<Type> GetRegisteredTypes()
        {
            return types;
        }

        public void RegisterType<T>()
        {
            types.Add(typeof(T));
        }

        public T Resolve<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
