using FMOD.API;
using FMOD.API.DamageHandles;
using FMOD.Events.Interfaces;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.EventArgs.Player
{
    public class HurtingEventArgs
    {
        public HurtingEventArgs(FirearmDamageHandler firearm, API.Player Target)
        {
            this.FirearmDamageHandler = new CustomFirearmDamage(firearm);
            this.Target = Target;
            this.IsAllow = true;
        }
        public CustomFirearmDamage FirearmDamageHandler;
        public API.Player Attacker
        {
            get => FirearmDamageHandler.Attacker;
        }
        public API.Player Target { get; set; }
        public float Damage
        {
            get => FirearmDamageHandler.Damage;
            set => FirearmDamageHandler.Damage = value;
        }
        public bool IsAllow { get; set; }
    }
}
