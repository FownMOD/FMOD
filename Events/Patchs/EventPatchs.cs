using CentralAuth;
using FMOD.API;
using FMOD.API.DamageHandles;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules.Misc;
using InventorySystem.Searching;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Patchs
{
    public class EventPatchs
    {

        [HarmonyPatch(typeof(PlayerAuthenticationManager), "FinalizeAuthentication")]
        [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnPlayerAdded))]
        [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnStartAuthority))]
        internal static class PlayerJoinPatch
        {
            private static void Prefix(ReferenceHub referenceHub)
            {
                Events.Handlers.Player.OnPlayerJoined(new EventArgs.Player.PlayerJoinArgs(referenceHub));
            }
        }
        [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnPlayerRemoved))]
        internal static class PlayerLeftPatch
        {
            private static void Prefix(ReferenceHub referenceHub)
            {
                var args = new Events.EventArgs.Player.PlayerLeftArgs(referenceHub);
                Events.Handlers.Player.OnPlayerLeft(args);
            }
        }
        [HarmonyPatch(typeof(PlayerRoleManager),nameof(PlayerRoleManager.OnServerRoleSet))]
        internal static class PlayerChangRole
        {
            private static void Prefix(ReferenceHub referenceHub)
            {
                var arg = new Events.EventArgs.Player.ChangingRoleArgs(referenceHub);
                Events.Handlers.Player.OnPlayerChangingRole(arg);
            }
        }
        [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.DealDamage))]
        internal static class PlayerHurtingPatch
        {
            private static bool Prefix(ReferenceHub __instance, DamageBase handler, ref bool __result)
            {
                try
                {
                    if (__instance == null || __instance == null || handler == null)
                        return true;

                    // 创建事件参数
                    var args = new Events.EventArgs.Player.HurtingEventArgs(__instance, handler);

                    // 触发事件
                    Events.Handlers.Player.OnPlayerHurting(args);

                    // 如果事件被取消，返回 false 并设置结果
                    if (!args.IsAllowed)
                    {
                        __result = false;
                        return false;
                    }

                    // 应用修改后的伤害
                    if (handler.BaseIs<StandardDamageHandler>(out var standardHandler))
                    {
                        standardHandler.Damage = args.Amount;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FMOD] PlayerHurtingPatch 错误: {ex.Message}");
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.OnThisPlayerDied))]
        internal static class PlayerDied
        {
            private static void Prefix(ReferenceHub referenceHub, DamageBase Dbase)
            {
                var args = new Events.EventArgs.Player.PlayerDiedEventsArgs(referenceHub, Dbase);
                Events.Handlers.Player.OnPlayerDied(args);
            }
        }
        [HarmonyPatch(typeof(Escape), nameof(Escape.OnServerPlayerEscape))]
        internal static class PlayerEsc
        {
            private static void Prefix(ReferenceHub referenceHub, RoleTypeId roleTypeId, Escape.EscapeScenarioType type)
            {
                var a = new EventArgs.Player.PlayerEscapeingArgs(referenceHub, roleTypeId, type);
                Events.Handlers.Player.OnPlayerEscapeing(a);
            }
        }
        [HarmonyPatch(typeof(PocketDimensionTeleport), nameof(PocketDimensionTeleport.OnPlayerEscapePocketDimension))]
        internal static class PlayerExitPocket
        {
            private static void Prefix(ReferenceHub referenceHub, PocketDimensionTeleport teleport)
            {
                var a = new EventArgs.Player.EscapingPocketDimensionEventArgs(teleport, referenceHub);
                Events.Handlers.Player.OnPlayerEscapingPocketDimension(a);
            }
        }
        [HarmonyPatch(typeof(ItemSearchCompletor), nameof(ItemSearchCompletor.Complete))]
        internal static class PickingUpItem
        {
            private static void Prefix(ReferenceHub hub, Pickup pickup)
            {
                var a = new EventArgs.Player.PlayerPickingArgs(hub, pickup);
                Events.Handlers.Player.OnPlayerPicking(a);
            }
        }
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.OnCurrentItemChanged))]
        internal static class ChangingItem
        {
            private static void Prefix(ReferenceHub referenceHub, Item item)
            {
                var a = new EventArgs.Player.ChangingItemArgs(referenceHub, item);
                Events.Handlers.Player.OnPlayerChangingItem(a);
            }
        }
        [HarmonyPatch(typeof(ShotBacktrackData), "ProcessShot")]
        internal static class Shooting
        {
            private static void Prefix(Firearm firearm, ShotBacktrackData d)
            {
                var a = new EventArgs.Player.ShootingEventArgs(firearm,ref d);
                Events.Handlers.Player.OnPlayerShooting(a);
            }
        }
    }
}
