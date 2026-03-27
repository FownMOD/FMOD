using FMOD.Events.EventArgs.Round;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Handlers
{
    public class Round
    {
        public static Action<RoundStartArgs> OnRoundStart;
        public static Action<RoundEndArgs> OnRoundEnd;
        public static void RoundStartInvoke(RoundStartArgs args)
        {
            OnRoundStart.Invoke(args);
        }
        public static void RoundEndInvoke(RoundEndArgs args)
        {
            OnRoundEnd.Invoke(args);
        }
    }
}
