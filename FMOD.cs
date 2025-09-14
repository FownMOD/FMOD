using FMOD.API;
using FMOD.Events.Patchs;
using FMOD.Other;
using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD
{
    public class FMODConfig
    {
        public bool IsEnabled { get; set; } = true;
        
    }
    public class FMOD : Plugin<FMODConfig>
    {
        public override string Name => "FMOD";

        public override string Description => "SCP SL Plugin Loader - FOMD";

        public override string Author => "灰";

        public override Version Version => new Version(FMODVersion.MainVersion.ToString());

        public override Version RequiredApiVersion => new Version(1,0);
        public Patcher Patcher { get; set; }
        public int ServerPort { get; set; } = ServerStatic.ServerPort;
        public override void Enable()
        {
            Harmony harmony = new Harmony($"{Name}.{Version}");
            Log.CustomInfo($"使用Harmony进行事件注册", UnityEngine.Color.gray);
            Log.CustomInfo($"===>{Name}.{Version}注册成功<===", UnityEngine.Color.gray);
            Paths.GenerateFoldersAndFiles(ServerPort);
            Load.LoadAllPlugins(ServerPort);
            Permissions.Initialize();
            Log.Debug($"欢迎使用FMOD");
            Log.Debug($"{LogMsg.FMOD}");
            Log.Debug($"一共加载了{Load.GetLoadedPlugins().Count}个插件");
        }
        public override void Disable()
        {
            Load.DisableAllPlugins();
            Log.CustomInfo($"FMOD已被禁用", UnityEngine.Color.red);
            Log.Debug($"{LogMsg.FMOD}");
        }
        /// <summary>
        /// Patches all events.
        /// </summary>
        public void Patch()
        {
            try
            {
                Patcher = new Patcher();
#if DEBUG
                bool lastDebugStatus = Harmony.DEBUG;
                Harmony.DEBUG = true;
#endif
                Patcher.PatchAll(false, out int failedPatch);

                if (failedPatch == 0)
                    Log.Debug("Events patched successfully!");
                else
                    Log.Error($"Patching failed! There are {failedPatch} broken patches.");
#if DEBUG
                Harmony.DEBUG = lastDebugStatus;
#endif
            }
            catch (Exception exception)
            {
                Log.Error($"Patching failed!\n{exception}");
            }
        }

        /// <summary>
        /// Unpatches all events.
        /// </summary>
        public void Unpatch()
        {
            Log.Debug("Unpatching events...");
            Patcher.UnpatchAll();
            Patcher = null;
            Log.Debug("All events have been unpatched complete. Goodbye!");
        }
    }
}
