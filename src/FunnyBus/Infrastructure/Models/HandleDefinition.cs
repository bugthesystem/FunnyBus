using System;

namespace FunnyBus.Infrastructure.Models
{
    internal class HandleDefinition
    {
        public string MessageFullName { get; set; }
        public Type MessageType { get; set; }
    }
}