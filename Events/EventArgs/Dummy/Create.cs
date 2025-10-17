using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Dummy
{
    public class Create:IFMODEvent
    {
        public Create(ReferenceHub referenceHub)
        {
            this.Player = API.Player.Get(referenceHub);
        }
        public API.Player Player { get; set; }
    }
}
