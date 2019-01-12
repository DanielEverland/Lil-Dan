using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lil_Dan.Event_Handlers
{
    public static class EventHandlerRegister
    {
        public static void RegisterEventHandlers()
        {
            IEnumerable<Type> eventHandlerTypes = typeof(IEventHandler).Assembly.GetTypes()
                .Where(x => typeof(IEventHandler).IsAssignableFrom(x))
                .Where(x => x.IsClass);

            foreach (Type type in eventHandlerTypes)
            {
                IEventHandler eventHandler = Activator.CreateInstance(type) as IEventHandler;

                eventHandler.Register();
            }
        }
    }
}
