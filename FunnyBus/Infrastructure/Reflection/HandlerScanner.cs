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

                foreach (Type definedType in executingAssembly.GetTypes())
                {
                    if (definedType.IsClass)
                    {
                        foreach (Type @interface in definedType.GetInterfaces())
                        {
//#if NET40
                            Type[] genericTypeArguments = @interface.GetGenericArguments();
//#else
//                            Type[] genericTypeArguments = @interface.GenericTypeArguments;
//#endif
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

                            if (genericType != null && @interface == genericType && genericType.IsAssignableFrom(definedType))
                            {
                                addToRegistry(definedType);
                                break;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}