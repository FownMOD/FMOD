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
        public static Event<PlayerPickiItemArgs> PlayerPicking { get; set; } = new Event<PlayerPickiItemArgs>();
        public static Event<PlayerLeftArgs> PlayerLeft { get; set; } = new Event<PlayerLeftArgs>();
        public static Event<PlayerEscapeingArgs> PlayerEscapeing { get; set; } = new Event<PlayerEscapeingArgs>();
        public static Event<EscapingPocketDimensionEventArgs> PlayerEscapingPocketDimension { get; set; } = new Event<EscapingPocketDimensionEventArgs>();
        public static Event<ChangingItemArgs> PlayerChangingItem { get; set; } = new Event<ChangingItemArgs>();
        public static Event<SpawnedRoleArgs> PlayerSpawnedRole { get; set; } = new Event<SpawnedRoleArgs>();
        public static Event<ShootingEventArgs> PlayerShooting { get; set; }=new Event<ShootingEventArgs>();
        public static Event<PlayerDiedEventsArgs> PlayerDied { get ; set; }= new Event<PlayerDiedEventsArgs>();
        public static Event<HurtingEventArgs> PlayerHurting { get; set; } = new Event<HurtingEventArgs>();
        /// <summary>
        /// 玩家射击事件
        /// </summary>
        public static void OnPlayerShooting(ShootingEventArgs ev)
        {
            PlayerShooting?.Invoke(ev);
        }
        public static void OnPlayerHurting(HurtingEventArgs ev)
        {
            PlayerHurting?.Invoke(ev);
        }
        public static void OnPlayerDied(PlayerDiedEventsArgs ev)
        {
            PlayerDied?.Invoke(ev);
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
        public static void OnPlayerPicking(PlayerPickiItemArgs args)
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
        /// 触发玩家切换物品事件
        /// </summary>
        public static void OnPlayerChangingItem(ChangingItemArgs args)
        {
            PlayerChangingItem.Invoke(args);
        }
        public static void InvokeSpawnedRole(SpawnedRoleArgs args)
        {
            PlayerSpawnedRole.Invoke(args);
        }
    }
}
