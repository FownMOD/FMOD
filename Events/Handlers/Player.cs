using FMOD.API;
using FMOD.API.Items;
using FMOD.Events.EventArgs.Player;
using FMOD.Events.Interfaces;
using InventorySystem.Items.Firearms.Modules.Misc;
using LabApi.Events.Arguments.PlayerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Handlers
{
    public class Player
    {
        public static Event<PlayerJoinArgs> PlayerJoined { get; set; } = new Event<PlayerJoinArgs>();
        public static Event<PlayerDroppingArgs> PlayerDropping { get; set; } = new Event<PlayerDroppingArgs>();
        public static Event<PlayerPickingArgs> PlayerPicking { get; set; } = new Event<PlayerPickingArgs>();
        public static Event<PlayerLeftArgs> PlayerLeft { get; set; } = new Event<PlayerLeftArgs>();
        public static Event<PlayerEscapeingArgs> PlayerEscapeing { get; set; } = new Event<PlayerEscapeingArgs>();
        public static Event<EscapingPocketDimensionEventArgs> PlayerEscapingPocketDimension { get; set; } = new Event<EscapingPocketDimensionEventArgs>();
        public static Event<HurtingEventArgs> PlayerHurting { get; set; } = new Event<HurtingEventArgs>();
        public static Event<ChangingItemArgs> PlayerChangingItem { get; set; } = new Event<ChangingItemArgs>();
        public static Event<ChangingRoleArgs> PlayerChangingRole { get; set; } = new Event<ChangingRoleArgs>();
        public static Event<PlayerDiedEventsArgs> PlayerDied { get; set; } = new Event<PlayerDiedEventsArgs>();
        public static Event<ShootingEventArgs> PlayerShooting { get; set; }=new Event<ShootingEventArgs>();
        /// <summary>
        /// 玩家射击事件
        /// </summary>
        public static void OnPlayerShooting(ShootingEventArgs ev)
        {
            PlayerShooting?.Invoke(ev);
        }
        /// <summary>
        /// 玩家死亡事件
        /// </summary>
        public static void OnPlayerDied(PlayerDiedEventsArgs args)
        {
            PlayerDied?.Invoke(args);
        }
        /// <summary>
        /// 触发玩家加入事件
        /// </summary>
        public static void OnPlayerJoined(PlayerJoinArgs args)
        {
            PlayerJoined.Invoke(args);
        }

        /// <summary>
        /// 触发玩家离开事件
        /// </summary>
        public static void OnPlayerLeft(PlayerLeftArgs args)
        {
            PlayerLeft.Invoke(args);
        }

        /// <summary>
        /// 触发玩家丢弃物品事件
        /// </summary>
        public static void OnPlayerDropping(PlayerDroppingArgs args)
        {
            PlayerDropping.Invoke(args);
        }

        /// <summary>
        /// 触发玩家拾取物品事件
        /// </summary>
        public static void OnPlayerPicking(PlayerPickingArgs args)
        {
            PlayerPicking.Invoke(args);
        }

        /// <summary>
        /// 触发玩家逃脱事件
        /// </summary>
        public static void OnPlayerEscapeing(PlayerEscapeingArgs args)
        {
            PlayerEscapeing.Invoke(args);
        }

        /// <summary>
        /// 触发玩家逃脱口袋空间事件
        /// </summary>
        public static void OnPlayerEscapingPocketDimension(EscapingPocketDimensionEventArgs args)
        {
            PlayerEscapingPocketDimension.Invoke(args);
        }

        /// <summary>
        /// 触发玩家受伤事件
        /// </summary>
        public static void OnPlayerHurting(HurtingEventArgs args)
        {
            PlayerHurting.Invoke(args);
        }

        /// <summary>
        /// 触发玩家切换物品事件
        /// </summary>
        public static void OnPlayerChangingItem(ChangingItemArgs args)
        {
            PlayerChangingItem.Invoke(args);
        }

        /// <summary>
        /// 触发玩家切换角色事件
        /// </summary>
        public static void OnPlayerChangingRole(ChangingRoleArgs args)
        {
            PlayerChangingRole.Invoke(args);
        }
    }
}
