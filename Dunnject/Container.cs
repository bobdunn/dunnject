using System;
using System.Collections.Generic;

namespace Dunnject
{
    public class Container
    {
        public IEnumerable<Type> GetRegisteredTypes()
        {
            return new List<Type>();
        }

        public void RegisterType<T>()
        {
        }
    }
}
