using System;
using System.Collections.Generic;

namespace Dunnject
{
    public class Container
    {
        public System.Collections.IEnumerable GetRegisteredTypes()
        {
            return new List<Type>();
        }

        public void RegisterType<T>()
        {
        }
    }
}
