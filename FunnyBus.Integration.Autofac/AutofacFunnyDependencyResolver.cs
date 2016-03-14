using System;
using System.Collections.Generic;
using Autofac;
using FunnyBus.Infrastructure.DependencyInjection;

namespace FunnyBus.Integration.Autofac
{
    public class AutofacFunnyDependencyResolver : IFunnyDependencyResolver
    {
        private readonly ILifetimeScope _rootScope;
        private List<AutofacFunnyDependencyResolver> _childs = new List<AutofacFunnyDependencyResolver>();

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="container">Autofac container instance</param>
        public AutofacFunnyDependencyResolver(ILifetimeScope container)
        {
            _rootScope = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType">Type to resolve.</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return _rootScope.Resolve(serviceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType">Type to resolve.</param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = _rootScope.Resolve(enumerableServiceType);

            return (IEnumerable<object>)instance;
        }

        public IFunnyDependencyResolver BeginNewScope()
        {
            AutofacFunnyDependencyResolver newScope = new AutofacFunnyDependencyResolver(_rootScope.BeginLifetimeScope());
            _childs.Add(newScope);
            return newScope;
        }

        public void Dispose()
        {
            if (_childs != null && _childs.Count > 0)
            {
                foreach (AutofacFunnyDependencyResolver dependencyScope in _childs)
                {
                    if (dependencyScope != null)
                    {
                        dependencyScope.Dispose();
                    }
                }
            }

            if (_rootScope != null)
            {
                _rootScope.Dispose();
            }
        }
    }
}