using FMOD.API;
using FMOD.API.Roles;
using FMOD.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class SpawnedRoleArgs:IFMODEvent
    {
        public SpawnedRoleArgs(ReferenceHub hub) 
        {
            this.Player = API.Player.Get(hub);
        }
        public API.Player Player { get; set; }
        public Role Role
        {
            get
            {
                return Player.Role;
            }
        }
    }
}
