using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Round
{
    public class RoundStartArgs:IFMODEvent
    {
        public RoundStartArgs(int PC)
        {
            this.PlayerCount = PC;
        }
        public int PlayerCount { get; }
    }
}
