using FMOD.API.Items;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.DamageHandles
{
    public class CustomFirearmDamage : DamageBase
    {
        public CustomFirearmDamage(FirearmDamageHandler damageHandler) : base(damageHandler)
        {
            this.Base = damageHandler;
        }
        public new FirearmDamageHandler Base { get; }
        public HitboxType Hitbox
        {
            get => Base.Hitbox;
        }
        public float TotalDamage
        {
            get => Base.TotalDamageDealt;
        }
        public ItemType AmmoType
        {
            get => Base.AmmoType;
        }
        public new float Damage
        {
            get => Base.Damage;
            set => Base.Damage = value;
        }
        public Item Firmeam
        {
            get
            {
                return Item.Get(Base.Firearm);
            }
        }
        public Player Attacker
        {
            get
            {
                return Player.Get(Base.Attacker);
            }
        }
        public Player Target { get; set; }
    }
}
