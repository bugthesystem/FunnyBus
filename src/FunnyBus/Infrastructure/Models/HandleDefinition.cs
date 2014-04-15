using System;

namespace FunnyBus.Infrastructure.Models
{
    internal class HandleDefinition
    {
        public string MessageFullName { get; set; }
        public Type MessageType { get; set; }
        public bool HasReturnType { get; set; }
        public Type ReturnType { get; set; }
    }
}