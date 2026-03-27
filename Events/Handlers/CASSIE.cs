using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Handlers
{
    public class CASSIE
    {
        public static Action<EventArgs.CASSIE.StartingArgs> OnStarting;
        public static void StartingInvoke(EventArgs.CASSIE.StartingArgs args)
        {
            OnStarting.Invoke(args);
        }
    }
}
