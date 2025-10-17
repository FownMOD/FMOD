using FMOD.API;
using FMOD.API.DamageHandles;
using FMOD.API.Items;
using FMOD.Events.EventArgs.Player;
using HarmonyLib;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Patchs
{
    public class PlayerPatchs
    {
        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnJoined))]
        public class PlayerJoined
        {
            static void Prefix(PlayerJoinedEventArgs ev)
            {
                PlayerJoinArgs playerJoinArgs = new PlayerJoinArgs(ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerJoined(playerJoinArgs); 
                if (ev.Player.ReferenceHub.TryGetComponent<DummyBase>(out var component))
                {
                    EventArgs.Dummy.Create Create = new EventArgs.Dummy.Create(ev.Player.ReferenceHub);
                    Handlers.Dummy.OnCreate(Create);
                }
            }
        }
        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnLeft))]
        public class PlayerLeft
        {
            static void Prefix(PlayerLeftEventArgs ev)
            {
                PlayerLeftArgs playerLeftArgs = new PlayerLeftArgs(ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerLeft(playerLeftArgs);
                if (ev.Player.ReferenceHub.TryGetComponent<DummyBase>(out var component))
                {
                    EventArgs.Dummy.Destroy destroy = new EventArgs.Dummy.Destroy(ev.Player.ReferenceHub);
                    Handlers.Dummy.OnDestroy(destroy);
                }
            }
        }
        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnEscaping))]
        public class PlayerEscing
        {
            static void Prefix(PlayerEscapingEventArgs ev)
            {
                PlayerEscapeingArgs playerEscapeingArgs = new PlayerEscapeingArgs(ev.Player.ReferenceHub, ev.NewRole, ev.EscapeScenario);
                Handlers.Player.OnPlayerEscapeing(playerEscapeingArgs);
            }
        }
        [HarmonyPatch(typeof(PlayerEvents), nameof(PlayerEvents.OnDroppingItem))]
        static class DroppingItem
        {
            static void Prefix(PlayerDroppingItemEventArgs ev)
            {
                PlayerDroppingArgs playerDroppingArgs = new PlayerDroppingArgs(ev.Player.ReferenceHub, Item.Get(ev.Item.Base));
                Handlers.Player.OnPlayerDropping(playerDroppingArgs);
            }
        }
        [HarmonyPatch(typeof(PlayerEvents), nameof(PlayerEvents.OnLeftPocketDimension))]
        static class LeftPocketDimension
        {
            static void Prefix(PlayerLeftPocketDimensionEventArgs ev)
            {
                EscapingPocketDimensionEventArgs escapingPocketDimensionEventArgs = new EscapingPocketDimensionEventArgs(ev.Teleport.Base, ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerEscapingPocketDimension(escapingPocketDimensionEventArgs);
            }
        }

        [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.DealDamage))]
        public class PlayerDiedAndHurting
        {
            static void Prefix(DamageHandlerBase handler)
            {
                DamageHandlerBase damageBase = handler;
                DamageBase damage = damageBase as DamageBase;
                DamageHandlerBase.HandlerOutput handlerOutput = damageBase.ApplyDamage(damage.Target.ReferenceHub);
                if (handlerOutput == DamageHandlerBase.HandlerOutput.Death)
                {
                    PlayerDiedEventsArgs playerDiedEvents = new PlayerDiedEventsArgs(damage.Target.ReferenceHub, damageBase);
                    Handlers.Player.OnPlayerDied(playerDiedEvents);
                }
                if (handlerOutput == DamageHandlerBase.HandlerOutput.Damaged)
                {
                    HurtingEventArgs hurtingEventArgs = new HurtingEventArgs(damage.Target.ReferenceHub, damageBase);
                    Handlers.Player.OnPlayerHurting(hurtingEventArgs);
                }
            }
        }
        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnChangingRole))]
        public class PlayerChangingRole
        {
            static void Prefix(PlayerChangingRoleEventArgs ev)
            {
                ChangingRoleArgs changingRoleArgs = new ChangingRoleArgs(ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerChangingRole(changingRoleArgs);
            }
        }

    }
}
