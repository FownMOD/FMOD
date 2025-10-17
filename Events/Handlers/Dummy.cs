using FMOD.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Handlers
{
    public class Dummy
    {
        public static event Action<EventArgs.Dummy.Create> CreateDummy;
        public static event Action<EventArgs.Dummy.Destroy> DestroyDummy;
        public static void OnDestroy(EventArgs.Dummy.Destroy d)
        {
            DestroyDummy?.Invoke(d);
        }
        public static void OnCreate(EventArgs.Dummy.Create dummy)
        {
            CreateDummy?.Invoke(dummy);
        }
    }
}
