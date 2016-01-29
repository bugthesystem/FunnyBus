using System;

namespace FunnyBus.Infrastructure
{
    internal interface IHandleDefinition
    {
        Type MessageType { get; set; }
    }
}