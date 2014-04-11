using System;

namespace FunnyBus.Infrastructure.IoC
{
    internal interface IPoorDependencyContainer
    {
        void Bind<TInterfaceType, TClassType>() where TClassType : TInterfaceType;
        void Bind<TInterfaceType>(object instance);
        void Bind<TInterface>(Func<object> factoryMethod);

        TKey Resolve<TKey>();
        object Resolve(Type type);

        void TeardownType<TType>();
        void TeardownContainer();
    }
}