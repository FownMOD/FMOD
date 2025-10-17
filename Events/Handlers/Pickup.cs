using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Handlers
{
    public class Pickup
    {
        public static event Action<EventArgs.Pickup.Create> Create;
        public static event Action<EventArgs.Pickup.Desroy> Destroy;
        public static void OnCreate(EventArgs.Pickup.Create create)
        {
            Create?.Invoke(create);
        }
        public static void OnDestroy(EventArgs.Pickup.Desroy destroy){ Destroy?.Invoke(destroy); }
    }
}
