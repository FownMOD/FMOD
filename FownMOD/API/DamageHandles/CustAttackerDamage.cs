using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.DamageHandles
{
    public class CustAttackerDamage : DamageBase
    {
        public CustAttackerDamage(AttackerDamageHandler damageHandler) : base(damageHandler)
        {
            this.Base = damageHandler;
        }
        public new AttackerDamageHandler Base { get; }
        public Player Attacker
        {
            get
            {
                return Player.Get(Base.Attacker);
            }
        }
        public bool IsFriendlyFire
        {
            get => Base.IsFriendlyFire;
        }
        public bool IsSuicide
        {
            get => Base.IsSuicide;
        }
        public HitboxType Hitbox
        {
            get => Base.Hitbox;
        }
    }
}
