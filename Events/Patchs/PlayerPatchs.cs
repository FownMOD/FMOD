using FMOD.API;
using FMOD.API.CustHint;
using FMOD.API.DamageHandles;
using FMOD.API.Items;
using FMOD.Events.EventArgs.Player;
using FMOD.Other;
using HarmonyLib;
using Hints;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Searching;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.Events.Patchs
{
    public class PlayerPatchs
    {
        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnJoined))]
        public static class LabJoinedPatch
        {
            [HarmonyPrefix]
            static void Prefix(PlayerJoinedEventArgs ev)
            {
                Player player = new Player(ev.Player.ReferenceHub);
                PlayerJoinArgs playerJoinArgs = new PlayerJoinArgs(player.ReferenceHub);
                Handlers.Player.OnPlayerJoined(playerJoinArgs);
                Player.List.Add(player);
                Player.Dictionary.Add(ev.Player.GameObject, player);
                Player.UnverifiedPlayers.Add(ev.Player.GameObject, player);
                Player.UserIdsCache.Add(ev.Player.UserId, player);
                if (!player.GameObject.TryGetComponent<PlayerHintController>(out _))
                {
                    player.GameObject.AddComponent<PlayerHintController>();
                }
                if (!player.GameObject.TryGetComponent<HintDisplay>(out _))
                {
                    player.GameObject.AddComponent<HintDisplay>();
                }
                Log.Debug($"玩家[{player.Nickname}]({player.UserId})加入服务器");
            }
        }
        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents),nameof(LabApi.Events.Handlers.PlayerEvents.OnLeft))]
        public static class LabLeftPatch
        {
            [HarmonyPrefix]
            static void Prefix(PlayerLeftEventArgs ev)
            {
                Player player = new Player(ev.Player.ReferenceHub);
                PlayerLeftArgs playerLeftArgs = new PlayerLeftArgs(player.ReferenceHub);
                Handlers.Player.OnPlayerLeft(playerLeftArgs);
                Player.List.Remove(player);
                Player.Dictionary.Remove(player.GameObject);
                Player.UnverifiedPlayers.Remove(player.GameObject);
                Player.UserIdsCache.Remove(player.UserId);
                Log.Debug($"玩家[{player.Nickname}]({player.UserId})离开服务器");
            }
        }
        [HarmonyPatch(typeof(PlayerStatsSystem.PlayerStats),nameof(PlayerStatsSystem.PlayerStats.DealDamage))]
        public class PlayerHrtingPatch
        {
            [HarmonyPrefix]
            static void Prefix(DamageHandlerBase handler)
            {
                AttackerDamageHandler attackerDamageHandler = handler as AttackerDamageHandler;
                HurtingEventArgs hurtingEventArgs = new HurtingEventArgs(attackerDamageHandler.Attacker.Hub, handler);
                Handlers.Player.OnPlayerHurting(hurtingEventArgs);
            }
        }
        [HarmonyPatch(typeof(PlayerRoleManager),nameof(PlayerRoleManager.InitializeNewRole))]
        public class PlayerSpawnedRolePatch
        {
            [HarmonyPostfix]
            static void Postfix(PlayerRoleManager __instance)
            {
                SpawnedRoleArgs spawnedRoleArgs = new SpawnedRoleArgs(__instance.Hub);
                Handlers.Player.InvokeSpawnedRole(spawnedRoleArgs);
            }
        }
        [HarmonyPatch(typeof(ItemSearchCompletor),nameof(ItemSearchCompletor.Complete))]
        public class PlayerPickingItem
        {
            [HarmonyPrefix]
            static bool Prefix()
            {
                ItemSearchCompletor itemSearchCompletor = (ItemSearchCompletor)Activator.CreateInstance(typeof(ItemSearchCompletor));
                PlayerPickiItemArgs playerPickiItemArgs = new PlayerPickiItemArgs(itemSearchCompletor.Hub, API.Pickup.Get(itemSearchCompletor.TargetPickup));
                if (playerPickiItemArgs.IsAllowed == false)
                {
                    return false;
                }
                Events.Handlers.Player.OnPlayerPicking(playerPickiItemArgs);
                return true;
            }
        }
    }
}