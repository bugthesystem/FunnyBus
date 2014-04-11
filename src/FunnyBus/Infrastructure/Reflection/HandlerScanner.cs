using System;
using System.Linq;
using System.Reflection;

namespace FunnyBus.Infrastructure.Reflection
{
    internal class HandlerScanner : IHandlerScanner
    {
        public bool RegisterHandlerDefinitions(Action<Type> addToRegistry)
        {
            foreach (Assembly executingAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (executingAssembly.DefinedTypes != null)
                {
                    foreach (TypeInfo definedType in executingAssembly.DefinedTypes)
                    {
                        if (definedType.IsClass)
                        {
                            foreach (Type @interface in definedType.GetInterfaces())
                            {
                                Type[] genericTypeArguments = @interface.GenericTypeArguments;
                                Type genericType = null;

                                switch (genericTypeArguments.Count())
                                {
                                    case 1:
                                        genericType = typeof(IHandle<>).MakeGenericType(genericTypeArguments.First());
                                        break;
                                    case 2:
                                        genericType = typeof(IHandle<,>).MakeGenericType(genericTypeArguments[0], genericTypeArguments[1]);
                                        break;
                                }

                                if (genericType != null && @interface == genericType && genericType.IsAssignableFrom(definedType.AsType()))
                                {
                                    addToRegistry(definedType.AsType());
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}