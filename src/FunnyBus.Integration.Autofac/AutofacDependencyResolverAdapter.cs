﻿using System;
using System.Collections.Generic;
using Autofac;
using FunnyBus.Infrastructure.IoC;

namespace FunnyBus.Integration.Autofac
{
    public class AutofacDependencyResolverAdapter : IDependencyResolverAdapter
    {
        private readonly IContainer _container;

        public AutofacDependencyResolverAdapter(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = _container.Resolve(enumerableServiceType);

            return (IEnumerable<object>)instance;
        }
    }
}