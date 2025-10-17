using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Dummy
{
    public class Destroy
    {
        public Destroy(ReferenceHub 
            hub)
        {
            this.Player = API.Player.Get(hub);
        }
        public API.Player Player { get; set; }
    }
}
