using FMOD.Events.Interfaces;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.Events.Patchs
{
    public class Patcher
    {
        /// <summary>
        /// The below variable is used to increment the name of the harmony instance, otherwise harmony will not work upon a plugin reload.
        /// </summary>
        private static int patchesCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Patcher"/> class.
        /// </summary>
        internal Patcher()
        {
            Harmony = new Harmony($"fmod.events.{++patchesCounter}");
        }

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> that contains all patch types that haven't been patched.
        /// </summary>
        public static HashSet<Type> UnpatchedTypes { get; private set; } = GetAllPatchTypes();

        /// <summary>
        /// Gets a set of types and methods for which FMOD patches should not be run.
        /// </summary>
        public static HashSet<MethodBase> DisabledPatchesHashSet { get; } = new HashSet<MethodBase>();

        /// <summary>
        /// Gets the <see cref="HarmonyLib.Harmony"/> instance.
        /// </summary>
        public Harmony Harmony { get; }

        /// <summary>
        /// Patches all events that target a specific <see cref="IFMODEvent"/>.
        /// </summary>
        /// <param name="event">The <see cref="IFMODEvent"/> all matching patches should target.</param>
        public void Patch(IFMODEvent @event)
        {
            try
            {
                var types = UnpatchedTypes.Where(x =>
                    x.GetCustomAttributes<EventPatchAttribute>().Any(epa => epa.Event == @event)).ToList();

                foreach (var type in types)
                {
                    var methodInfos = new PatchClassProcessor(Harmony, type).Patch();
                    if (methodInfos.Any(DisabledPatchesHashSet.Contains))
                        ReloadDisabledPatches();

                    UnpatchedTypes.Remove(type);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] Patching by event failed!\n{ex}");
            }
        }

        /// <summary>
        /// Patches all events.
        /// </summary>
        /// <param name="includeEvents">Whether to patch events as well as all required patches.</param>
        /// <param name="failedPatch">the number of failed patch returned.</param>
        public void PatchAll(bool includeEvents, out int failedPatch)
        {
            failedPatch = 0;

            try
            {
                var toPatch = includeEvents ?
                    UnpatchedTypes.ToList() :
                    UnpatchedTypes.Where(type => !type.GetCustomAttributes<EventPatchAttribute>().Any()).ToList();

                foreach (var patch in toPatch)
                {
                    try
                    {
                        Harmony.CreateClassProcessor(patch).Patch();
                        UnpatchedTypes.Remove(patch);
                    }
                    catch (HarmonyException exception)
                    {
                        Console.WriteLine($"[FMOD] Patching by attributes failed!\n{exception}");
                        failedPatch++;
                    }
                }

                Console.WriteLine("[FMOD] Events patched by attributes successfully!");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[FMOD] Patching by attributes failed!\n{exception}");
            }
        }

        /// <summary>
        /// Checks the <see cref="DisabledPatchesHashSet"/> list and un-patches any methods that have been defined there. Once un-patching has been done, they can be patched by plugins, but will not be re-patchable by FMOD until a server reboot.
        /// </summary>
        public void ReloadDisabledPatches()
        {
            foreach (var method in DisabledPatchesHashSet)
            {
                Harmony.Unpatch(method, HarmonyPatchType.All, Harmony.Id);
                Console.WriteLine($"[FMOD] Unpatched {method.Name}");
            }
        }

        /// <summary>
        /// Unpatches all events.
        /// </summary>
        public void UnpatchAll()
        {
            Console.WriteLine("[FMOD] Unpatching events...");
            Harmony.UnpatchAll(Harmony.Id);
            UnpatchedTypes = GetAllPatchTypes();
            Console.WriteLine("[FMOD] All events have been unpatched.");
        }

        /// <summary>
        /// Gets all types that have a <see cref="HarmonyPatch"/> attributed to them.
        /// </summary>
        /// <returns>A <see cref="HashSet{T}"/> of all patch types.</returns>
        internal static HashSet<Type> GetAllPatchTypes()
        {
            try
            {
                return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type => type.GetCustomAttributes<HarmonyPatch>().Any())
                    .ToHashSet();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] Failed to get patch types: {ex.Message}");
                return new HashSet<Type>();
            }
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventPatchAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPatchAttribute"/> class.
        /// </summary>
        /// <param name="event">The event this patch is associated with.</param>
        public EventPatchAttribute(IFMODEvent @event)
        {
            Event = @event;
        }

        /// <summary>
        /// Gets the event this patch is associated with.
        /// </summary>
        public IFMODEvent Event { get; }
    }
}
