using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dunnject
{
    public class Container
    {
        Dictionary<string, List<TypeContainer>> types = new Dictionary<string, List<TypeContainer>>();

        public IEnumerable<string> GetRegisteredTypes()
        {
            return types.Keys;
        }

        public void RegisterType<T>(LifecycleType lifecycleType = LifecycleType.Transient)
        {
            RegisterType<T, T>(lifecycleType);
        }

        public void RegisterType<TAbstract, TConcrete>(LifecycleType lifecycleType = LifecycleType.Transient)
        {
            RegisterType(typeof(TAbstract), typeof(TConcrete), lifecycleType);
        }

        public void RegisterType(Type abstractType, Type concreteType, LifecycleType lifecycleType = LifecycleType.Transient)
        {
            string name = GetName(abstractType);
            if (!types.ContainsKey(name))
            {
                types[name] = new List<TypeContainer>();
            }
            types[name].Add(new TypeContainer(abstractType, concreteType, lifecycleType));
        }

        public IEnumerable<object> ResolveMultiple(Type type)
        {
            string name = GetName(type);
            List<TypeContainer> typeContainers;
            if (!types.TryGetValue(name, out typeContainers))
            {
                throw new TypeLoadException();
            }
            foreach (TypeContainer typeContainer in typeContainers)
            {
                if (typeContainer.Lifecycle == LifecycleType.Singleton)
                {
                    if (typeContainer.Instance == null)
                    {
                        typeContainer.Instance = GetInstance(typeContainer.AbstractType);
                    }
                    yield return typeContainer.Instance;
                }

                yield return GetInstance(typeContainer.AbstractType);
            }
        }

        string GetName(Type type)
        {
            return $"{type.Namespace}.{type.Name}";
        }

        public void RegisterType(Type abstractType, object instance)
        {
            string name = GetName(abstractType);
            if (!types.ContainsKey(name))
            {
                types[name] = new List<TypeContainer>();
            }
            types[name].Add(new TypeContainer(abstractType, instance));
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            string name = GetName(type);
            List<TypeContainer> typeContainers;
            if (!types.TryGetValue(name, out typeContainers))
            {
                throw new TypeLoadException();
            }
            var typeContainer = typeContainers[0];
            if (typeContainer.Lifecycle == LifecycleType.Singleton)
            {
                if (typeContainer.Instance == null)
                {
                    typeContainer.Instance = GetInstance(typeContainer.AbstractType);
                }
                return typeContainer.Instance;
            }

            return GetInstance(typeContainer.AbstractType);
        }

        private HashSet<string> resolvingTypes = new HashSet<string>();
        private object GetInstance(Type type)
        {
            var containers = types[GetName(type)];
            var singleton = containers.FirstOrDefault(c => c.Instance != null);
            if (singleton != null)
            {
                return singleton.Instance;
            }
            foreach (var container in containers.Where(c=>c.Instance==null))
            {
                try
                {
                    var args = container.GetDependencies().Select(d =>
                    {
                        string name = GetName(d);

                        if (types.ContainsKey(name) && !resolvingTypes.Contains(name))
                        {
                            resolvingTypes.Add(name);
                            var dependentType = Resolve(d);
                            resolvingTypes.Remove(name);
                            return dependentType;
                        }
                        else { throw new TypeLoadException($"Failed to load {container.ConcreteType}"); }
                    }).ToArray();

                    return Activator.CreateInstance(container.ConcreteType, args);
                } catch { }
            }
            throw new TypeLoadException();
        }
    }
}