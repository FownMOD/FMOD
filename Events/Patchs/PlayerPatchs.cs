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
            static bool Prefix(PlayerJoinedEventArgs ev)
            {
                PlayerJoinArgs playerJoinArgs = new PlayerJoinArgs(ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerJoined(playerJoinArgs);

                if (ev.Player.ReferenceHub.TryGetComponent<DummyBase>(out var component))
                {
                    EventArgs.Dummy.Create Create = new EventArgs.Dummy.Create(ev.Player.ReferenceHub);
                    Handlers.Dummy.OnCreate(Create);
                }

                return true; 
            }
        }

        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnLeft))]
        public class PlayerLeft
        {
            static bool Prefix(PlayerLeftEventArgs ev)
            {
                PlayerLeftArgs playerLeftArgs = new PlayerLeftArgs(ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerLeft(playerLeftArgs);

                if (ev.Player.ReferenceHub.TryGetComponent<DummyBase>(out var component))
                {
                    EventArgs.Dummy.Destroy destroy = new EventArgs.Dummy.Destroy(ev.Player.ReferenceHub);
                    Handlers.Dummy.OnDestroy(destroy);
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnEscaping))]
        public class PlayerEscing
        {
            static bool Prefix(PlayerEscapingEventArgs ev)
            {
                PlayerEscapeingArgs playerEscapeingArgs = new PlayerEscapeingArgs(ev.Player.ReferenceHub, ev.NewRole, ev.EscapeScenario);
                Handlers.Player.OnPlayerEscapeing(playerEscapeingArgs);
                return true; 
            }
        }

        [HarmonyPatch(typeof(PlayerEvents), nameof(PlayerEvents.OnDroppingItem))]
        static class DroppingItem
        {
            static bool Prefix(PlayerDroppingItemEventArgs ev)
            {
                PlayerDroppingArgs playerDroppingArgs = new PlayerDroppingArgs(ev.Player.ReferenceHub, Item.Get(ev.Item.Base));
                Handlers.Player.OnPlayerDropping(playerDroppingArgs);
                return true; 
            }
        }

        [HarmonyPatch(typeof(PlayerEvents), nameof(PlayerEvents.OnLeftPocketDimension))]
        static class LeftPocketDimension
        {
            static bool Prefix(PlayerLeftPocketDimensionEventArgs ev)
            {
                EscapingPocketDimensionEventArgs escapingPocketDimensionEventArgs = new EscapingPocketDimensionEventArgs(ev.Teleport.Base, ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerEscapingPocketDimension(escapingPocketDimensionEventArgs);
                return true;
            }
        }

        [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.DealDamage))]
        public class PlayerDiedAndHurting
        {
            static bool Prefix(DamageHandlerBase handler)
            {
                if (handler is DamageBase damage)
                {
                    DamageHandlerBase.HandlerOutput handlerOutput = handler.ApplyDamage(damage.Target.ReferenceHub);

                    if (handlerOutput == DamageHandlerBase.HandlerOutput.Death)
                    {
                        PlayerDiedEventsArgs playerDiedEvents = new PlayerDiedEventsArgs(damage.Target.ReferenceHub, handler);
                        Handlers.Player.OnPlayerDied(playerDiedEvents);
                    }
                    else if (handlerOutput == DamageHandlerBase.HandlerOutput.Damaged)
                    {
                        HurtingEventArgs hurtingEventArgs = new HurtingEventArgs(damage.Target.ReferenceHub, handler);
                        Handlers.Player.OnPlayerHurting(hurtingEventArgs);
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(LabApi.Events.Handlers.PlayerEvents), nameof(LabApi.Events.Handlers.PlayerEvents.OnChangingRole))]
        public class PlayerChangingRole
        {
            static bool Prefix(PlayerChangingRoleEventArgs ev)
            {
                ChangingRoleArgs changingRoleArgs = new ChangingRoleArgs(ev.Player.ReferenceHub);
                Handlers.Player.OnPlayerChangingRole(changingRoleArgs);
                return true; 
            }
        }
    }
}