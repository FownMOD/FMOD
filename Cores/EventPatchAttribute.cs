using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Cores
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventPatchAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPatchAttribute"/> class.
        /// </summary>
        /// <param name="eventHandlerType">The event handler type (e.g., typeof(Events.Handlers.Player)).</param>
        /// <param name="eventArgsType">The event args type (e.g., typeof(Events.EventArgs.Player.HurtingEventArgs)).</param>
        public EventPatchAttribute(Type eventHandlerType, Type eventArgsType)
        {
            EventHandlerType = eventHandlerType;
            EventArgsType = eventArgsType;
        }

        /// <summary>
        /// Gets the event handler type.
        /// </summary>
        public Type EventHandlerType { get; }

        /// <summary>
        /// Gets the event args type.
        /// </summary>
        public Type EventArgsType { get; }

        /// <summary>
        /// Gets the event name for display purposes.
        /// </summary>
        public string EventName => $"{EventHandlerType?.Name}.{EventArgsType?.Name}";
    }
}
