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
    public class PlayerDiedEventsArgs : IFMODPlayerEvent
    {
        public PlayerDiedEventsArgs(ReferenceHub target, DamageHandlerBase damageHandlerBase)
        {
            Player = API.Player.Get(target);

            // 创建 DamageBase 包装器
            DamageBase = new DamageBase
            {
                Base = damageHandlerBase,
                Target = Player
            };

            // 尝试获取攻击者
            if (damageHandlerBase is AttackerDamageHandler attackerDamage)
            {
                Attacker = API.Player.Get(attackerDamage.Attacker.Hub);
                DamageBase.Attacker = Attacker;
            }

            IsAllowed = true;
        }

        public DamageBase DamageBase { get; }
        public API.Player Player { get; set; }
        public API.Player Attacker { get; }
        public bool IsAllowed { get; set; }
    }
}
