using FMOD.Cores;
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
        /// Gets the <see cref="HarmonyLib.Harmony"/> instance.
        /// </summary>
        public Harmony Harmony { get; }

        /// <summary>
        /// Patches all events that target a specific event handler and event args.
        /// </summary>
        /// <param name="eventHandlerType">The event handler type.</param>
        /// <param name="eventArgsType">The event args type.</param>
        public void Patch(Type eventHandlerType, Type eventArgsType)
        {
            try
            {
                var types = UnpatchedTypes.Where(x =>
                    x.GetCustomAttributes<EventPatchAttribute>()
                     .Any(epa => epa.EventHandlerType == eventHandlerType &&
                                 epa.EventArgsType == eventArgsType))
                     .ToList();

                foreach (var type in types)
                {
                    Harmony.CreateClassProcessor(type).Patch();
                    UnpatchedTypes.Remove(type);
                    Console.WriteLine($"[FMOD] 已打补丁: {type.Name} -> {eventHandlerType.Name}.{eventArgsType.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] 按事件打补丁失败!\n{ex}");
            }
        }

        /// <summary>
        /// Patches all events for a specific event handler.
        /// </summary>
        /// <param name="eventHandlerType">The event handler type.</param>
        public void Patch(Type eventHandlerType)
        {
            try
            {
                var types = UnpatchedTypes.Where(x =>
                    x.GetCustomAttributes<EventPatchAttribute>()
                     .Any(epa => epa.EventHandlerType == eventHandlerType))
                     .ToList();

                foreach (var type in types)
                {
                    var attribute = type.GetCustomAttributes<EventPatchAttribute>()
                        .First(epa => epa.EventHandlerType == eventHandlerType);

                    Harmony.CreateClassProcessor(type).Patch();
                    UnpatchedTypes.Remove(type);
                    Console.WriteLine($"[FMOD] 已打补丁: {type.Name} -> {attribute.EventName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] 按事件处理器打补丁失败!\n{ex}");
            }
        }

        /// <summary>
        /// Patches all events.
        /// </summary>
        /// <param name="failedPatch">the number of failed patch returned.</param>
        public void PatchAll(out int failedPatch)
        {
            failedPatch = 0;

            try
            {
                var toPatch = UnpatchedTypes.ToList();

                foreach (var patch in toPatch)
                {
                    try
                    {
                        var attributes = patch.GetCustomAttributes<EventPatchAttribute>();
                        Harmony.CreateClassProcessor(patch).Patch();
                        UnpatchedTypes.Remove(patch);

                        foreach (var attribute in attributes)
                        {
                            Console.WriteLine($"[FMOD] 已打补丁: {patch.Name} -> {attribute.EventName}");
                        }
                    }
                    catch (HarmonyException exception)
                    {
                        Console.WriteLine($"[FMOD] 打补丁失败!\n{exception}");
                        failedPatch++;
                    }
                }

                Console.WriteLine("[FMOD] 所有补丁已成功应用!");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[FMOD] 打补丁失败!\n{exception}");
            }
        }

        /// <summary>
        /// Unpatches all events.
        /// </summary>
        public void UnpatchAll()
        {
            Console.WriteLine("[FMOD] 移除补丁...");
            Harmony.UnpatchAll(Harmony.Id);
            UnpatchedTypes = GetAllPatchTypes();
            Console.WriteLine("[FMOD] 所有补丁已移除");
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
                    .Where(type => type.GetCustomAttributes<HarmonyPatch>().Any() ||
                                   type.GetCustomAttributes<EventPatchAttribute>().Any())
                    .ToHashSet();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FMOD] 获取补丁类型失败: {ex.Message}");
                return new HashSet<Type>();
            }
        }
    }

    /// <summary>
    /// Static access to the patcher.
    /// </summary>
    public static class PatcherAccess
    {
        private static Patcher _patcher;

        /// <summary>
        /// Gets the patcher instance.
        /// </summary>
        public static Patcher Patcher => _patcher = new Patcher();

        /// <summary>
        /// Patches all events.
        /// </summary>
        public static void PatchAll()
        {
            Patcher.PatchAll(out int failedPatches);
            if (failedPatches > 0)
            {
                Console.WriteLine($"[FMOD] {failedPatches} 个补丁应用失败");
            }
        }

        /// <summary>
        /// Patches events for a specific event handler and event args.
        /// </summary>
        public static void Patch(Type eventHandlerType, Type eventArgsType)
        {
            Patcher.Patch(eventHandlerType, eventArgsType);
        }

        /// <summary>
        /// Patches all events for a specific event handler.
        /// </summary>
        public static void Patch(Type eventHandlerType)
        {
            Patcher.Patch(eventHandlerType);
        }

        /// <summary>
        /// Unpatches all events.
        /// </summary>
        public static void UnpatchAll()
        {
            Patcher.UnpatchAll();
        }
    }
}
