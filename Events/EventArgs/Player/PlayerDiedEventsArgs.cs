using FMOD.API;
using FMOD.API.DamageHandles;
using FMOD.Events.Interfaces;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class PlayerDiedEventsArgs
    {
        public PlayerDiedEventsArgs(FirearmDamageHandler firearmDamageHandler, API.Player Target) 
        {
            this.DamageHandler = new CustomFirearmDamage(firearmDamageHandler);
            this.Target = Target;
        }
        public CustomFirearmDamage DamageHandler { get; set; }
        public API.Player Attacker
        {
            get
            {
                return DamageHandler.Attacker;
            }
        }
        public API.Player Target { get; set; }
    }
}
